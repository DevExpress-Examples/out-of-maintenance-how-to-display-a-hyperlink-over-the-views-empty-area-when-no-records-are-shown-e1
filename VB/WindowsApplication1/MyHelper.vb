Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.Utils
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base

Namespace WindowsApplication1
	Public Class MyHelper

		Private _ActiveView As GridView
		Public Property ActiveView() As GridView
			Get
				Return _ActiveView
			End Get
			Set(ByVal value As GridView)
				_ActiveView = value
			End Set
		End Property
		Public Sub New(ByVal view As GridView)
			ActiveView = view
			SubscribeEvents()

		End Sub



		Private _Font As Font
		Public Property PaintFont() As Font
			Get
				Return If(_Font Is Nothing, AppearanceObject.DefaultFont, _Font)
			End Get
			Set(ByVal value As Font)
				_Font = value
			End Set
		End Property

		Public ReadOnly Property ActiveGridControl() As GridControl
			Get
				Return ActiveView.GridControl
			End Get
		End Property

		Private _NoMatchesFoundText As String
		Public ReadOnly Property NoMatchesFoundText() As String
			Get
				Return "No matches found"
			End Get
		End Property


		 Public ReadOnly Property TrySearchingAgainText() As String
			Get
				Return "Try searching again"
			End Get
		 End Property

		Private Sub SubscribeEvents()
			AddHandler ActiveView.CustomDrawEmptyForeground, AddressOf ActiveView_CustomDrawEmptyForeground
			AddHandler ActiveView.MouseMove, AddressOf ActiveView_MouseMove
			AddHandler ActiveView.MouseDown, AddressOf ActiveView_MouseDown
		End Sub

		Private Sub ActiveView_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			If InTrySearchingAgainBounds() Then
				MessageBox.Show("Test")
			End If
		End Sub

		Private Sub ActiveView_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			ActiveGridControl.Cursor = If(InTrySearchingAgainBounds(), Cursors.Hand, Cursors.Default)
			ActiveGridControl.Invalidate(TrySearchingAgainBounds(GetForegroundBounds()))
		End Sub

		Private Function GetLinkFont() As Font
			Dim fs As FontStyle = FontStyle.Underline
			If InTrySearchingAgainBounds() Then
				fs = fs Or FontStyle.Bold
			End If
			Return New Font(PaintFont, fs)
		End Function

		Private Sub ActiveView_CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
			DrawNoMatchesFound(e)
			DrawTrySearchingAgain(e)
		End Sub


		Private Function GetStringSize(ByVal s As String, ByVal font As Font) As Size
			Dim g As Graphics = Graphics.FromHwnd(ActiveGridControl.Handle)
			Return g.MeasureString(s, font).ToSize()
		End Function


		Private Function NoMatchesFoundBounds(ByVal bounds As Rectangle) As Rectangle
			Dim size As Size = GetStringSize(NoMatchesFoundText, PaintFont)
			Dim x As Integer = (bounds.Width - size.Width) \ 2
			Dim y As Integer = bounds.Y + 10
			Return New Rectangle(New Point(x, y), size)
		End Function

		Private Function TrySearchingAgainBounds(ByVal bounds As Rectangle) As Rectangle
			Dim r As Rectangle = NoMatchesFoundBounds(bounds)
			Dim x As Integer = r.X
			Dim y As Integer = r.Bottom + 10
			Dim s As Size = GetStringSize(TrySearchingAgainText, PaintFont)
			s.Width += s.Width \5
			Return New Rectangle(New Point(x, y), s)
		End Function

		Private Function GetForegroundBounds() As Rectangle
			Return (TryCast(ActiveView.GetViewInfo(), GridViewInfo)).ViewRects.Rows
		End Function

		Private Function InTrySearchingAgainBounds() As Boolean
			Dim p As Point = ActiveGridControl.PointToClient(Control.MousePosition)
			Return TrySearchingAgainBounds(GetForegroundBounds()).Contains(p)
		End Function

		Private Sub DrawNoMatchesFound(ByVal e As CustomDrawEventArgs)
			e.Cache.DrawString(NoMatchesFoundText, PaintFont, Brushes.Gray, NoMatchesFoundBounds(e.Bounds), e.Appearance.GetStringFormat())
		End Sub

		Private Sub DrawTrySearchingAgain(ByVal e As CustomDrawEventArgs)
			e.Cache.DrawString(TrySearchingAgainText, GetLinkFont(), Brushes.Blue, TrySearchingAgainBounds(e.Bounds), e.Appearance.GetStringFormat())
		End Sub

	End Class
End Namespace
