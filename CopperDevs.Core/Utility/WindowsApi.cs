using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Drawing;
using System.Numerics;
using CopperDevs.Core.Data;

namespace CopperDevs.Core.Utility;

public static partial class WindowsApi
{
    public static Action<Vector2Int> OnWindowResize = null!;

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsWindows11 => Environment.OSVersion.Version.Build >= 22000;

    private static int IntSize => sizeof(int);
    private const int WM_SIZE = 0x0005;
    private const int GWLP_WNDPROC = -4;

    private static WndProc newWndProc;
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

    public static void SetDwmWindowAttribute(IntPtr windowHandle, WindowAttribute dwAttribute, int pvAttribute)
    {
        if (IsWindows && IsWindows11)
            DwmSetWindowAttribute(windowHandle, dwAttribute, ref pvAttribute, IntSize);
    }

    public static void SetDwmImmersiveDarkMode(IntPtr windowHandle, bool enableDarkMode)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.UseImmersiveDarkMode, enableDarkMode.ToInt());
    }

    public static void SetDwmSystemBackdropType(IntPtr windowHandle, SystemBackdropType backdropType)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.SystemBackdropType, (int)backdropType);
    }

    public static void SetDwmWindowCornerPreference(IntPtr windowHandle, WindowCornerPreference preference)
    {
        if (IsWindows && IsWindows11)
            SetDwmWindowAttribute(windowHandle, WindowAttribute.WindowCornerPreference, (int)preference);
    }

    public static void ExtendFrameIntoClientArea(IntPtr windowHandle, Margins margins)
    {
        if (IsWindows && IsWindows11)
            DwmExtendFrameIntoClientArea(windowHandle, ref margins);
    }

    public static IntPtr GetConsoleWindowPointer()
    {
        unsafe
        {
            return IsWindows ? GetConsoleWindow() : new IntPtr(null);
        }
    }

    public static void SetWindowState(IntPtr targetWindow, WindowState state)
    {
        if (IsWindows)
            ShowWindow(targetWindow, (int)state);
    }

    public static void SetWindowParent(IntPtr child, IntPtr parent)
    {
        if (IsWindows)
            SetParent(child, parent);
    }

    public static Point GetMousePosition()
    {
        GetCursorPos(out var point);
        return point;
    }

    public static void RegisterWindow(IntPtr windowHandle)
    {
        // Subclass the window
        newWndProc = CustomWndProc;
        oldWndProc = SetWindowLongPtr(windowHandle, GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newWndProc));

        // Restore the original window procedure before exiting
        // SetWindowLongPtr(hwnd, GWLP_WNDPROC, oldWndProc);

        return;

        IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg != WM_SIZE)
                return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);

            var width = lParam.ToInt32() & 0xFFFF;
            var height = lParam.ToInt32() >> 16;

            OnWindowResize?.Invoke(new Vector2Int(width, height));

            // Call the original window procedure for default processing
            return CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }
    }

    public static (int X, int Y, int Width, int Height) GetWindowPosition(IntPtr hWnd)
    {
        if (!GetWindowRect(hWnd, out var rect)) 
            throw new InvalidOperationException("Unable to get window position.");
        
        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;
        return (rect.Left, rect.Top, width, height);

    }

    public enum WindowAttribute
    {
        UseImmersiveDarkMode = 20,
        WindowCornerPreference = 33,
        SystemBackdropType = 38
    }

    public enum SystemBackdropType
    {
        Auto,
        None,
        Mica,
        Acrylic,
        Tabbed
    }

    public enum WindowCornerPreference
    {
        Default,
        DoNotRound,
        Round,
        RoundSmall,
    }

    public enum WindowState
    {
        Hide,
        ShowNormal,
        ShowMinimized,
        ShowMaximized,
        ShowNoActivate,
        Show,
        Minimize,
        ShowMinNoActive,
        ShowNa,
        Restore,
        ShowDefault,
        ForceMinimize
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int CxLeftWidth;
        public int CxRightWidth;
        public int CyTopHeight;
        public int CyBottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;

        public static implicit operator Vector2Int(Point point) => new(point.x, point.y);
        public static implicit operator Vector2(Point point) => new(point.x, point.y);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}