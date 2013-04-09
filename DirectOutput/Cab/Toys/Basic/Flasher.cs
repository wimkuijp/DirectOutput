﻿using DirectOutput.Cab.Toys.Generic;

namespace DirectOutput.Cab.Toys.Basic
{
    /// <summary>
    /// The Flasher toy fires one or several short pluses/flashes on the configured IOutput at given intervalls.
    /// </summary>
    public class Flasher : GenericDigitalToy, IToy
    {
        private int _DefaultIntervallMs=150;

        /// <summary>
        /// Gets or sets the default intervall between flashes in milliseconds.<br/>
        /// Default value of this property is 150 milliseconds.
        /// </summary>
        /// <value>
        /// The default intervall in milliseconds.
        /// </value>
        public int DefaultIntervallMs
        {
            get { return _DefaultIntervallMs; }
            set { _DefaultIntervallMs = value; }
        }

        private int _FlashDurationMs=20;

        /// <summary>
        /// Gets or sets the flash duration in milliseconds.<br/>
        /// Default value of this property is set to 20ms or the value of Pinball.UpdateTimer.IntervalMs if this value is above 20 milliseconds.
        /// </summary>
        /// <value>
        /// The flash duration in milliseconds.
        /// </value>
        public int FlashDurationMs
        {
            get { return _FlashDurationMs; }
            set { _FlashDurationMs = value; }
        }



        /// <summary>
        /// Fires a single flash.
        /// </summary>
        public void Fire()
        {
            Fire(1);
        }

        /// <summary>
        /// Fires the specified number of flashes.<br/>
        /// The value of DefaultIntervallMs will be used for the intervall between flashes.
        /// </summary>
        /// <param name="NumberOfFlashes">The number of flashes.</param>
        public void Fire(int NumberOfFlashes)
        {
            Fire(NumberOfFlashes, DefaultIntervallMs);
        }

        private int RemainingFlashes = 0;
        private int FlashIntervallMs = 0;

        /// <summary>
        /// Fires the specified number of flashes.
        /// </summary>
        /// <param name="NumberOfFlashes">The number of flashes.</param>
        /// <param name="IntervallMs">The intervall between flashes in milliseconds.</param>
        public void Fire(int NumberOfFlashes, int IntervallMs)
        {
            if (NumberOfFlashes < 1) return;

            RemainingFlashes = NumberOfFlashes;
            FlashIntervallMs = IntervallMs;

            SetState(true);
            RemainingFlashes--;

            UpdateTimer.RegisterAlarm(FlashDurationMs, FlasherOn);

        }

        private void FlasherOn()
        {
            SetState(false);
            if (RemainingFlashes > 0)
            {
                UpdateTimer.RegisterAlarm(FlashIntervallMs, FlasherOff);
            }
        }

        private void FlasherOff()
        {
            if (RemainingFlashes > 0)
            {
                SetState(true);
                RemainingFlashes--;
                UpdateTimer.RegisterAlarm(FlashDurationMs, FlasherOn);
            }
        }



        private UpdateTimer UpdateTimer;

        /// <summary>
        /// Initalizes the Flasher.
        /// </summary>
        /// <param name="Pinball"><see cref="Pinball" /> object containing the <see cref="Cabinet" /> to which the <see cref="Flasher" /> belongs.</param>
        public override void Init(Pinball Pinball)
        {
            UpdateTimer = Pinball.UpdateTimer;
            if (FlashDurationMs < UpdateTimer.IntervalMs) { FlashDurationMs = UpdateTimer.IntervalMs; }
            base.Init(Pinball);
        }

        /// <summary>
        /// Finishes the Flasher toy and releases used references.
        /// </summary>
        public override void Finish()
        {
            RemainingFlashes=0;
            UpdateTimer=null;
            base.Finish();
        }

    }
}