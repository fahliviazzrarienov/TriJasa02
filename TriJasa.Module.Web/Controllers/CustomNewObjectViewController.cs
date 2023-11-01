using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.SystemModule;

namespace TriJasa.Module.Web.Controllers
{
    public class CustomNewObjectViewController : WebNewObjectViewController {
        protected override void New(DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventArgs args) {
            if(Frame is NestedFrame) {
                FrameRoudMapController frameRoudMap = Application.MainWindow.GetController<FrameRoudMapController>();
                frameRoudMap.AddFrame(Frame);
            }
            base.New(args);
        }
    }
    public class FrameRoudMapController : WindowController {
        private Stack<Frame> framesMap = new Stack<Frame>();

        public void AddFrame(Frame frame) {
            if(!framesMap.Contains(frame)) {
                framesMap.Push(frame);
                frame.Disposed += frame_Disposed;
            }
        }
        public Frame ParentFrame {
            get {
                return framesMap.Peek();
            }
        }

        private void frame_Disposed(object sender, EventArgs e) {
            Frame frame = framesMap.Pop();
            frame.Disposed -= frame_Disposed;
        }
    }
}
