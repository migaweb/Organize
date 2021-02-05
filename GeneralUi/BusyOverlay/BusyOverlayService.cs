﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUi.BusyOverlay
{
  public class BusyOverlayService
  {
    public event EventHandler<BusyChangedEventArgs> BusyStateChanged;

    public BusyEnum CurrentBusyState { get; set; }

    public void SetBusyState(BusyEnum busyState)
    {
      CurrentBusyState = busyState;
      var eventArgs = new BusyChangedEventArgs();
      eventArgs.BusyState = CurrentBusyState;
      OnBusyStateChanged(eventArgs);
    }

    protected virtual void OnBusyStateChanged(BusyChangedEventArgs e)
    {
      var handler = BusyStateChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }
  }
}
