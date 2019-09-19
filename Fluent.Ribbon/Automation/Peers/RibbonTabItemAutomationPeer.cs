﻿namespace Fluent.Automation.Peers
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using JetBrains.Annotations;

    /// <summary>
    /// Automation peer wrapper for <see cref="RibbonTabItem"/>.
    /// </summary>
    public class RibbonTabItemAutomationPeer : FrameworkElementAutomationPeer
    {
        private RibbonTabItem OwningTab => (RibbonTabItem)this.Owner;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RibbonTabItemAutomationPeer([NotNull] RibbonTabItem owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
            List<AutomationPeer> list = null;
            if (this.OwningTab.IsSelected)
            {
                //list = base.GetChildrenCore();
                list = new List<AutomationPeer>(this.OwningTab.Groups.Count);
                foreach (var @group in this.OwningTab.Groups)
                {
                    var peer = CreatePeerForElement(@group);

                    if (peer != null)
                    {
                        list.Add(peer);
                    }
                }
            }

            return list;
        }

        /// <inheritdoc />
        protected override string GetClassNameCore()
        {
            return this.Owner.GetType().Name;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void RaiseTabExpandCollapseAutomationEvent(bool oldValue, bool newValue)
        {
            this.EventsSource?.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void RaiseTabSelectionEvents()
        {
            var eventsSource = this.EventsSource;
            if (eventsSource != null)
            {
                if (this.OwningTab.IsSelected)
                {
                    eventsSource.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
                }
                else
                {
                    eventsSource.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
                }
            }
        }
    }
}