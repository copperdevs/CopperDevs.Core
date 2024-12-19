using System.Runtime.InteropServices;
using CopperDevs.Core.Data;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Windows api utility
/// </summary>
public static partial class WindowsApi
{
    /// <summary>
    /// Method called whenever a registered window is resized
    /// </summary>
    /// <remarks>
    /// For this callback to be ran you must first register your window with <see cref="RegisterWindow"/>
    /// </remarks>
    public static Action<Vector2Int> OnWindowResize = null!;

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Is true when the program is running on windows
    /// </summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Is true when the program is running on windows 11
    /// </summary>
    public static bool IsWindows11 => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && Environment.OSVersion.Version.Build >= 22000;

    private static int IntSize => sizeof(int);
    private const int WmSize = 0x0005;
    private const int GwlpWndproc = -4;

    private static WndProc newWndProc = null!;
    private static IntPtr oldWndProc;


    [LibraryImport("dwmapi.dll")]
    private static partial void DwmSetWindowAttribute(IntPtr window, WindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);

    [LibraryImport("dwmapi.dll")]
    private static partial void DwmExtendFrameIntoClientArea(IntPtr window, ref Margins pMarInset);

    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetConsoleWindow();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [LibraryImport("user32.dll")]
    private static partial IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrA")]
    private static partial IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [LibraryImport("user32.dll", EntryPoint = "CallWindowProcA")]
    private static partial IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetCursorPos(out Point lpPoint);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

    /// <summary>
    /// Set a Desktop Windows Management Window Attribute
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <param name="dwAttribute">Attribute to set</param>
    /// <param name="pvAttribute">Value of the attribute to set</param>
    public static void SetDwmWindowAttribute(IntPtr windowHandle, WindowAttribute dwAttribute, int pvAttribute)
    {
        if (IsWindows && IsWindows11)
            DwmSetWindowAttribute(windowHandle, dwAttribute, ref pvAttribute, IntSize);
    }

    /// <summary>
    /// Set the windows immersive dark mode state
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <param name="enableDarkMode">Dark mode enabled</param>
    public static void SetDwmImmersiveDarkMode(IntPtr windowHandle, bool enableDarkMode)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.UseImmersiveDarkMode, enableDarkMode.ToInt());
    }

    /// <summary>
    /// Change the windows backdrop type
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <param name="backdropType">New window backdrop type</param>
    public static void SetDwmSystemBackdropType(IntPtr windowHandle, SystemBackdropType backdropType)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.SystemBackdropType, (int)backdropType);
    }

    /// <summary>
    /// Set the windows corner preference
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <param name="preference">New window corner preference</param>
    public static void SetDwmWindowCornerPreference(IntPtr windowHandle, WindowCornerPreference preference)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.WindowCornerPreference, (int)preference);
    }

    /// <summary>
    /// Extends the window frame into the client area.
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <param name="margins">New margins value</param>
    public static void ExtendFrameIntoClientArea(IntPtr windowHandle, Margins margins)
    {
        if (IsWindows && IsWindows11)
            DwmExtendFrameIntoClientArea(windowHandle, ref margins);
    }

    /// <summary>
    /// Get the handle of the connected console window if the program is a console app
    /// </summary>
    /// <returns>Handle of the connected console window</returns>
    public static IntPtr GetConsoleWindowPointer()
    {
        unsafe
        {
            return IsWindows ? GetConsoleWindow() : new IntPtr(null);
        }
    }

    /// <summary>
    /// Set a windows state
    /// </summary>
    /// <param name="targetWindow">Handle of the target window</param>
    /// <param name="state">State to set</param>
    public static void SetWindowState(IntPtr targetWindow, WindowState state)
    {
        if (IsWindows)
            ShowWindow(targetWindow, (int)state);
    }

    /// <summary>
    /// Parent a window to another
    /// </summary>
    /// <param name="childWindow">Child window handle</param>
    /// <param name="parentWindow">Parent window handle</param>
    public static void SetWindowParent(IntPtr childWindow, IntPtr parentWindow)
    {
        if (IsWindows)
            SetParent(childWindow, parentWindow);
    }

    /// <summary>
    /// Get the position of the mouse cursor in position of the monitors
    /// </summary>
    /// <returns>Mouse position in monitor space</returns>
    public static SystemVector2 GetMousePosition()
    {
        GetCursorPos(out var point);
        return point;
    }

    /// <summary>
    /// Register a window to call the <see cref="OnWindowResize"/> callback
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    public static void RegisterWindow(IntPtr windowHandle)
    {
        // Subclass the window
        newWndProc = CustomWndProc;
        oldWndProc = SetWindowLongPtr(windowHandle, GwlpWndproc, Marshal.GetFunctionPointerForDelegate(newWndProc));

        // Restore the original window procedure before exiting
        // SetWindowLongPtr(hwnd, GwlpWndproc, oldWndProc);

        return;

        IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg != WmSize)
                return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);

            var width = lParam.ToInt32() & 0xFFFF;
            var height = lParam.ToInt32() >> 16;

            OnWindowResize?.Invoke(new Vector2Int(width, height));

            // Call the original window procedure for default processing
            return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }
    }

    /// <summary>
    /// Get the position of a specified window in monitor space
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <returns>Position of the window</returns>
    public static SystemVector2 GetWindowPosition(IntPtr windowHandle)
    {
        if (!GetWindowRect(windowHandle, out var rect))
            throw new InvalidOperationException("Unable to get window position.");

        return new SystemVector2(rect.Left, rect.Top);
    }

    /// <summary>
    /// Get the size of a specified window
    /// </summary>
    /// <param name="windowHandle">Handle of the target window</param>
    /// <returns>Size of the widnow</returns>
    public static SystemVector2 GetWindowSize(IntPtr windowHandle)
    {
        if (!GetWindowRect(windowHandle, out var rect))
            throw new InvalidOperationException("Unable to get window size.");

        return new SystemVector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WindowAttribute
    {
        /// <summary>
        /// Allows the window frame for this window to be drawn in dark mode colors when the dark mode system setting is enabled. For compatibility reasons, all windows default to light mode regardless of the system setting.
        /// </summary>
        UseImmersiveDarkMode = 20,

        /// <summary>
        /// Specifies the rounded corner preference for a window.
        /// </summary>
        WindowCornerPreference = 33,

        /// <summary>
        /// The system-drawn backdrop material of a window, including behind the non-client area.
        /// </summary>
        SystemBackdropType = 38
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SystemBackdropType
    {
        /// <summary>
        /// The default. Let the Desktop Window Manager (DWM) automatically decide the system-drawn backdrop material for this window. This applies the backdrop material just behind the default Win32 title bar. This behavior attempts to preserve maximum backwards compatibility. For this reason, the DWM might also decide to draw no backdrop material at all based on internal heuristics. If drawing the backdrop material behind the entire window is required, choose one of the other more specific values of this enum as appropriate 
        /// </summary>
        Auto,

        /// <summary>
        /// Don't draw any system backdrop.
        /// </summary>
        None,

        /// <summary>
        /// Draw the backdrop material effect corresponding to a long-lived window behind the entire window bounds. For Windows 11, this corresponds to Mica in its default variant. The material effect might change with future Windows releases. For more info about Mica, see <see href="https://learn.microsoft.com/en-us/windows/apps/design/style/mica"/>
        /// </summary>
        Mica,

        /// <summary>
        /// Draw the backdrop material effect corresponding to a transient window behind the entire window bounds. For Windows 11, this corresponds to Desktop Acrylic, also known as Background Acrylic, in its brightest variant. The material effect might change with future Windows releases. For more info about Desktop Acrylic, see <see href="https://learn.microsoft.com/en-us/windows/apps/design/style/acrylic"/>
        /// </summary>
        Acrylic,

        /// <summary>
        /// Draw the backdrop material effect corresponding to a window with a tabbed title bar behind the entire window bounds. For Windows 11, this corresponds to Mica in its alternate variant (Mica Alt). The material might change with future releases of Windows. For more info about Mica Alt, see <see href="https://learn.microsoft.com/en-us/windows/apps/design/style/mica#app-layering-with-mica-alt"/>
        /// </summary>
        Tabbed
    }

    /// <summary>
    /// Specifies the rounded corner preference for a window.
    /// </summary>
    public enum WindowCornerPreference
    {
        /// <summary>
        /// Let the system decide when to round window corners.
        /// </summary>
        Default,

        /// <summary>
        /// Never round window corners.
        /// </summary>
        DoNotRound,

        /// <summary>
        /// Round the corners, if appropriate.
        /// </summary>
        Round,

        /// <summary>
        /// Round the corners if appropriate, with a small radius.
        /// </summary>
        RoundSmall,
    }

    /// <summary>
    /// States a window can be
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide,

        /// <summary>
        /// Activates and displays a window. If the window is minimized, maximized, or arranged, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        ShowNormal,

        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized,

        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>
        ShowMaximized,

        /// <summary>
        /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
        /// </summary>
        ShowNoActivate,

        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        Show,

        /// <summary>
        /// Minimizes the specified window and activates the next top-level window in the Z order.
        /// </summary>
        Minimize,

        /// <summary>
        /// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated.
        /// </summary>
        ShowMinNoActive,

        /// <summary>
        /// Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated.
        /// </summary>
        ShowNa,

        /// <summary>
        /// Activates and displays the window. If the window is minimized, maximized, or arranged, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore,

        /// <summary>
        /// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
        /// </summary>
        ShowDefault,

        /// <summary>
        /// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize
    }

    /// <summary>
    /// Margin
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        /// <summary>
        /// Width of the left border that retains its size.
        /// </summary>
        public int CxLeftWidth;

        /// <summary>
        /// Width of the right border that retains its size.
        /// </summary>
        public int CxRightWidth;

        /// <summary>
        /// Height of the top border that retains its size.
        /// </summary>
        public int CyTopHeight;

        /// <summary>
        /// Height of the bottom border that retains its size.
        /// </summary>
        public int CyBottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Point
    {
        public int x;
        public int y;

        public static implicit operator Vector2Int(Point point) => new(point.x, point.y);
        public static implicit operator SystemVector2(Point point) => new(point.x, point.y);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
