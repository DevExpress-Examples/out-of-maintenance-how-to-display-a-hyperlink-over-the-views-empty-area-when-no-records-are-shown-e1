using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;

namespace WindowsApplication1
{
    public class MyHelper
    {

        private GridView _ActiveView;
        public GridView ActiveView
        {
            get { return _ActiveView; }
            set { _ActiveView = value; }
        }
        public MyHelper(GridView view)
        {
            ActiveView = view;
            SubscribeEvents();

        }

    

        private Font _Font;
        public Font PaintFont
        {
            get { return _Font == null? AppearanceObject.DefaultFont : _Font; }
            set { _Font = value; }
        }

        public GridControl ActiveGridControl
        {
            get { return ActiveView.GridControl; }
        }

        private string _NoMatchesFoundText;
        public string NoMatchesFoundText
        {
            get { return "No matches found"; }
        }
       
        
         public string TrySearchingAgainText
        {
            get { return "Try searching again"; }
        }

        void SubscribeEvents()
        {
            ActiveView.CustomDrawEmptyForeground += new CustomDrawEventHandler(ActiveView_CustomDrawEmptyForeground);
            ActiveView.MouseMove += new MouseEventHandler(ActiveView_MouseMove);
            ActiveView.MouseDown += new MouseEventHandler(ActiveView_MouseDown);
        }

        void ActiveView_MouseDown(object sender, MouseEventArgs e)
        {
            if (InTrySearchingAgainBounds()) MessageBox.Show("Test");
        }

        void ActiveView_MouseMove(object sender, MouseEventArgs e)
        {
            ActiveGridControl.Cursor = InTrySearchingAgainBounds() ? Cursors.Hand : Cursors.Default;
            ActiveGridControl.Invalidate(TrySearchingAgainBounds(GetForegroundBounds()));
        }

        Font GetLinkFont()
        {
            FontStyle fs = FontStyle.Underline;
            if (InTrySearchingAgainBounds()) fs = fs | FontStyle.Bold;
            return new Font(PaintFont, fs);
        }

        void ActiveView_CustomDrawEmptyForeground(object sender, CustomDrawEventArgs e)
        {
            DrawNoMatchesFound(e);
            DrawTrySearchingAgain(e);
        }


        Size GetStringSize(string s, Font font)
        {
            Graphics g = Graphics.FromHwnd(ActiveGridControl.Handle);
            return g.MeasureString(s, font).ToSize();
        }


        Rectangle NoMatchesFoundBounds(Rectangle bounds)
        {
            Size size = GetStringSize(NoMatchesFoundText, PaintFont);
            int x = (bounds.Width - size.Width) / 2;
            int y = bounds.Y + 10;
            return new Rectangle(new Point(x, y), size);
        }

        Rectangle TrySearchingAgainBounds(Rectangle bounds)
        {
            Rectangle r = NoMatchesFoundBounds(bounds);
            int x = r.X;
            int y = r.Bottom + 10;
            Size s = GetStringSize(TrySearchingAgainText, PaintFont);
            s.Width += s.Width /5;
            return new Rectangle(new Point(x, y), s );
        }

        Rectangle GetForegroundBounds()
        {
            return (ActiveView.GetViewInfo() as GridViewInfo).ViewRects.Rows;
        }

        bool InTrySearchingAgainBounds()
        {
            Point p = ActiveGridControl.PointToClient(Control.MousePosition);
            return TrySearchingAgainBounds(GetForegroundBounds()).Contains(p);
        }

        void DrawNoMatchesFound(CustomDrawEventArgs e)
        {
            e.Graphics.DrawString(NoMatchesFoundText, PaintFont, Brushes.Gray, NoMatchesFoundBounds(e.Bounds).Location);
        }

        void DrawTrySearchingAgain(CustomDrawEventArgs e)
        {
            e.Graphics.DrawString(TrySearchingAgainText, GetLinkFont(), Brushes.Blue, TrySearchingAgainBounds(e.Bounds).Location);
        }

    }
}
