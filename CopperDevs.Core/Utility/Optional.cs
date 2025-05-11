namespace CopperDevs.Core.Utility;

/// <summary>
/// Represents an optional value that may or may not be set.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public struct Optional<TValue>
{
    /// <summary>
    /// Gets a value indicating whether this <see cref="Optional{TValue}"/> contains a value.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets the value
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="Enabled"/> is false.</exception>
    public TValue Value
    {
        get
        {
            if (!Enabled)
                throw new InvalidOperationException("Optional is disabled.");
            return value!;
        }
        set
        {
            if (!Enabled)
                throw new InvalidOperationException("Optional is disabled.");
            this.value = value;
        }
    }

    private TValue? value;

    /// <summary>
    /// Initializes a new instance of the <see cref="Optional{TValue}"/> struct with the specified value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    public Optional(TValue value)
    {
        Enabled = true;
        this.value = value;
    }

    private Optional(bool enabled, TValue? value)
    {
        Enabled = enabled;
        this.value = value;
    }

    /// <summary>
    /// Returns an empty <see cref="Optional{TValue}"/> instance.
    /// </summary>
    public static Optional<TValue> Default => new(false, default);

    /// <summary>
    /// Implicitly wraps a value into an <see cref="Optional{TValue}"/>.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    public static implicit operator Optional<TValue>(TValue value) => new(value);

    /// <summary>
    /// Attempts to get the value.
    /// </summary>
    /// <param name="value">The output value if present and <see cref="Enabled"/> is set to true.</param>
    /// <returns>True if the value is present and enabled; otherwise, false.</returns>
    // ReSharper disable once ParameterHidesMember
    public bool TryGetValue(out TValue? value)
    {
        if (!Enabled)
        {
            value = default;
            return false;
        }

        value = this.value;
        return Enabled;
    }
}