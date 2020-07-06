using LemonUI.Elements;
using System;
using System.Drawing;

namespace LemonUI.Items
{
    /// <summary>
    /// A slider item for changing integer values.
    /// </summary>
    public class NativeSliderItem : NativeSlidableItem
    {
        #region Internal Fields

        /// <summary>
        /// The background of the slider.
        /// </summary>
        internal protected ScaledRectangle background = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(255, 4, 32, 57)
        };
        /// <summary>
        /// THe front of the slider.
        /// </summary>
        internal protected ScaledRectangle slider = new ScaledRectangle(PointF.Empty, SizeF.Empty)
        {
            Color = Color.FromArgb(255, 57, 116, 200)
        };

        #endregion

        #region Private Fields

        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        private int maximum = 0;
        /// <summary>
        /// The current value of the slider.
        /// </summary>
        private int _value = 100;

        #endregion

        #region Public Properties

        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        public int Maximum
        {
            get => maximum;
            set
            {
                // If the value was not changed, return
                if (maximum == value)
                {
                    return;
                }
                // Otherwise, save the new value
                maximum = value;
                // If the current value is higher than the max, set the max
                if (_value > maximum)
                {
                    _value = maximum;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
                // Finally, update the location of the slider
                UpdatePosition();
            }
        }
        /// <summary>
        /// The current value of the slider.
        /// </summary>
        public int Value
        {
            get => _value;
            set
            {
                // If the value is over the limit, raise an exception
                if (value > maximum)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"The value is over the maximum of {maximum - 1}");
                }
                // Otherwise, save it
                _value = value;
                // Trigger the respective event
                ValueChanged?.Invoke(this, EventArgs.Empty);
                // And update the location of the slider
                UpdatePosition();
            }
        }
        /// <summary>
        /// The multiplier for increasing and decreasing the value.
        /// </summary>
        public int Multiplier { get; set; } = 1;

        #endregion

        #region Event

        /// <summary>
        /// Event triggered when the value of the menu changes.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Constructors

        public NativeSliderItem(string title) : this(title, "", 100, 0)
        {
        }

        public NativeSliderItem(string title, string subtitle) : this(title, subtitle, 100, 0)
        {
        }

        public NativeSliderItem(string title, int max, int value) : this(title, "", max, value)
        {
        }

        public NativeSliderItem(string title, string subtitle, int max, int value) : base(title, subtitle)
        {
            // Save the values
            maximum = max;
            _value = value;
        }

        #endregion

        #region Internal Functions

        /// <summary>
        /// Updates the position of the bar based on the value.
        /// </summary>
        internal protected void UpdatePosition()
        {
            // Calculate the increment, and then the value of X
            float increment = _value / (float)maximum;
            float x = (background.Size.Width - slider.Size.Width) * increment;
            // Then, add the X to the slider position
            slider.Position = new PointF(background.Position.X + x, background.Position.Y);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Reduces the value of the slider.
        /// </summary>
        public override void GoLeft()
        {
            // Calculate the new value
            int newValue = _value - Multiplier;
            // If is under zero, set it to zero
            if (newValue < 0)
            {
                Value = 0;
            }
            // Otherwise, set it to the new value
            else
            {
                Value = newValue;
            }
        }
        /// <summary>
        /// Increases the value of the slider.
        /// </summary>
        public override void GoRight()
        {
            // Calculate the new value
            int newValue = _value + Multiplier;
            // If the value is over the maximum, set the max
            if (newValue > maximum)
            {
                Value = maximum;
            }
            // Otherwise, set the calculated value
            else
            {
                Value = newValue;
            }
        }
        /// <summary>
        /// Draws the slider.
        /// </summary>
        public override void Draw()
        {
            // Draw all arrows
            base.Draw();
            // And draw the background with the slider
            background.Draw();
            slider.Draw();
        }

        #endregion
    }
}