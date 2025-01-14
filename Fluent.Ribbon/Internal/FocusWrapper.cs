﻿namespace Fluent.Internal
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Windows.Win32;
    using Windows.Win32.Foundation;

    internal class FocusWrapper
    {
        private readonly IInputElement? inputElement;
        private readonly IntPtr handle;

        private FocusWrapper(IInputElement inputElement)
        {
            this.inputElement = inputElement;
        }

        private FocusWrapper(IntPtr handle)
        {
            this.handle = handle;
        }

        public void Focus()
        {
            if (this.inputElement is not null)
            {
                this.inputElement.Focus();
                return;
            }

            if (this.handle != IntPtr.Zero)
            {
#pragma warning disable 618
                PInvoke.SetFocus(new HWND(this.handle));
#pragma warning restore 618
            }
        }

        public static FocusWrapper? GetWrapperForCurrentFocus()
        {
            if (Keyboard.FocusedElement is not null)
            {
                return new FocusWrapper(Keyboard.FocusedElement);
            }

#pragma warning disable 618
            var handle = PInvoke.GetFocus();
#pragma warning restore 618

            if (handle != IntPtr.Zero)
            {
                return new FocusWrapper(handle);
            }

            return null;
        }
    }
}