using System;
using System.Drawing;
using System.Windows.Forms;

namespace Jw_Quiz_Development
{
    /// <summary>
    /// Shared legacy hint behavior for static story forms:
    /// - shows a placeholder hint image while hidden
    /// - pulses the hint slot until revealed
    /// - toggles reveal/hide text and image state
    /// </summary>
    internal sealed class LegacyHintAnimator : IDisposable
    {
        private readonly PictureBox hintBox;
        private readonly Button hintButton;
        private readonly Timer pulseTimer;
        private readonly Image originalHintImage;
        private readonly Image placeholderHintImage;
        private bool isRevealed;
        private bool pulseGrow;

        public LegacyHintAnimator(PictureBox hintBox, Button hintButton)
        {
            this.hintBox = hintBox;
            this.hintButton = hintButton;

            originalHintImage = hintBox != null ? hintBox.Image : null;
            placeholderHintImage = StoryResources.GetImage(StoryResources.KeyHint) ?? originalHintImage;

            pulseTimer = new Timer { Interval = 300 };
            pulseTimer.Tick += PulseTimer_Tick;

            InitializeHiddenState();
        }

        public void Toggle()
        {
            if (hintBox == null || hintButton == null)
            {
                return;
            }

            if (!isRevealed)
            {
                RevealHint();
            }
            else
            {
                InitializeHiddenState();
            }
        }

        private void RevealHint()
        {
            isRevealed = true;
            hintButton.Text = "Nascondi indizio";
            hintBox.Visible = true;
            hintBox.Image = originalHintImage ?? placeholderHintImage;
            hintBox.BackColor = Color.FromArgb(34, 52, 77);
            pulseTimer.Stop();
        }

        private void InitializeHiddenState()
        {
            isRevealed = false;
            if (hintButton != null)
            {
                hintButton.Text = "Rivela indizio";
            }

            if (hintBox != null)
            {
                hintBox.Visible = true;
                hintBox.Image = placeholderHintImage;
                hintBox.BackColor = Color.FromArgb(34, 52, 77);
            }

            pulseGrow = false;
            pulseTimer.Start();
        }

        private void PulseTimer_Tick(object sender, EventArgs e)
        {
            if (isRevealed || hintBox == null || hintBox.IsDisposed)
            {
                pulseTimer.Stop();
                return;
            }

            pulseGrow = !pulseGrow;
            hintBox.BackColor = pulseGrow
                ? Color.FromArgb(180, 130, 0)
                : Color.FromArgb(34, 52, 77);
        }

        public void Dispose()
        {
            if (pulseTimer != null)
            {
                pulseTimer.Stop();
                pulseTimer.Dispose();
            }
        }
    }
}
