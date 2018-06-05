'===============================================================================
'   Description  : 印刷処理
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On
Module mPrint
	
	Private Const cTwipToMili As Single = 54.7 '[mm]→[twip]
	'マージン
	Private Const cTop As Single = 12.5 '[mm]
	Private Const cBottom As Single = 12.5 '[mm]
	Private Const cLeft As Single = 25# '[mm]
	Private Const cRight As Single = 12.5 '[mm]
	
	Private mTop As Integer '[twip]
	Private mBottom As Integer '[twip]
	Private mLeft As Integer '[twip]
	Private mRight As Integer '[twip]
	
	Private cx As Integer
	Private cy As Integer
	Private tWidth As Integer
	Private tHeight As Integer
	
	Private sCrLf As String
	
	Private fInit As Boolean
	
	Public Function Z_LPRINT(ByRef pMsg As String) As Integer
		
		Z_LPRINT = 0
		Debug.Print("LPT:" & pMsg)
		
		pMsg = pMsg & vbCrLf
		
		Call mPrint.pMsgPrint(pMsg)
		
	End Function
	
	Public Function Z_LPRINT_ENDDOC() As Integer
		
		Call mPrint.pEndDoc()
		
	End Function
	
	
	'-------------------------------------------------
	Private Function printerInit() As Short
		
		printerInit = 0
		
		On Error GoTo ErrExit
		
		
        '' ''UPGRADE_ISSUE: 定数 vbPRPSA4 はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.PaperSize はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''Printer.PaperSize = vbPRPSA4

        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.FontSize はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''Printer.FontSize = 10.5

        '' ''マージン設定
        ' ''mTop = cTop * cTwipToMili
        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.Height はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''mBottom = Printer.Height - (cBottom * cTwipToMili)
        ' ''mLeft = cLeft * cTwipToMili
        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.width はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''mRight = Printer.Width - (cRight * cTwipToMili)

        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''Printer.CurrentX = mLeft
        '' ''UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        ' ''Printer.CurrentY = mTop
		cx = mLeft
		cy = mTop
		tWidth = 0
		tHeight = 0
		
		On Error GoTo 0
		
		fInit = True
		
		Exit Function
		
ErrExit: 
		Resume ErrExit2
ErrExit2: 
        On Error GoTo 0
        Call Form1.System1.TrmMsgBox(gSysPrm, "Printer initialize error.", MsgBoxStyle.OkOnly, gAppName)
        Form1.Refresh()
		printerInit = 1
		fInit = False
		
	End Function
	
	
	'-------------------------------------------------
	Private Function PutLength(ByRef nStr As String) As Integer
		
        'Dim i As Integer
        'Dim iLen As Integer
        'Dim n As String
		
        'i = 1 : iLen = mLeft
        'Do 
        '	n = Mid(nStr, i, 1)
        '	'UPGRADE_ISSUE: Printer メソッド Printer.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '	iLen = iLen + Printer.TextWidth(n)

        '	If n = vbCr Or n = vbLf Then
        '		i = i - 1
        '		Exit Do
        '	End If

        '	If mRight <= iLen Then
        '		sCrLf = vbCrLf
        '		i = i - 1
        '		Exit Do
        '	End If

        '	i = i + 1
        'Loop While i < Len(nStr)
        'PutLength = i
		
	End Function
	
	
	'-------------------------------------------------
	Private Function PutString(ByRef gStr As String) As String
		
		Dim n As Integer
		Dim pStr As String
		
		sCrLf = ""
		pStr = Mid(gStr, 1, 1)
		If pStr = vbCr Or pStr = vbLf Then
			n = 1
		Else
			n = PutLength(gStr)
		End If
		
		pStr = Left(gStr, n)
		gStr = sCrLf & Mid(gStr, n + 1)
		PutString = pStr
		
		Debug.Print("n:" & n)
		
	End Function
	
	
	'-------------------------------------------------
	Private Sub pMsgPrint(ByRef s As String)
		
        ''UPGRADE_NOTE: str は str_Renamed にアップグレードされました。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"' をクリックしてください。
        'Dim str_Renamed As String
        'Dim r As Short

        'If fInit = False Then
        '	r = printerInit
        '	If (r <> 0) Then
        '		Exit Sub
        '	End If
        'End If

        'Do 
        '	str_Renamed = PutString(s)

        '	If str_Renamed = vbCr Then
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentX = mLeft
        '		Debug.Print("vbCr")
        '	ElseIf str_Renamed = vbLf Then 
        '		'UPGRADE_ISSUE: Printer メソッド Printer.TextHeight はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentY = Printer.CurrentY + Printer.TextHeight("")
        '		Debug.Print("vbLf")
        '	Else
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		cx = Printer.CurrentX
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		cy = Printer.CurrentY
        '		'UPGRADE_ISSUE: Printer メソッド Printer.TextWidth はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		tWidth = Printer.TextWidth(str_Renamed)
        '		'UPGRADE_ISSUE: Printer メソッド Printer.TextHeight はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		tHeight = Printer.TextHeight(str_Renamed)
        '		'UPGRADE_ISSUE: Printer オブジェクト はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"' をクリックしてください。
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Print はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Print(str_Renamed)
        '		Debug.Print(str_Renamed)
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentX = cx + tWidth
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentY = cy
        '	End If

        '	'UPGRADE_ISSUE: Printer メソッド Printer.TextHeight はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '	'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '	If mBottom < Printer.CurrentY + Printer.TextHeight("") Then
        '		Call pNextPage()
        '	End If
        'Loop While s <> ""
	End Sub
	
	
	'-------------------------------------------------
	Private Sub pNextPage()
        '		If fInit = False Then printerInit()

        '#If cOFFLINEcDEBUG Then
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mTop) - (mRight, mTop))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mBottom) - (mRight, mBottom))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mTop) - (mLeft, mBottom))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mRight, mTop) - (mRight, mBottom))
        '#End If

        '		'UPGRADE_ISSUE: Printer メソッド Printer.NewPage はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.NewPage()
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentX = mLeft
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentY = mTop
	End Sub
	
	
	'-------------------------------------------------
	Private Sub pEndDoc()
        '		If fInit = False Then Exit Sub

        '#If cOFFLINEcDEBUG Then
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mTop) - (mRight, mTop))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mBottom) - (mRight, mBottom))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mLeft, mTop) - (mLeft, mBottom))
        '		'UPGRADE_ISSUE: Printer メソッド Printer.Line はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.Line((mRight, mTop) - (mRight, mBottom))
        '#End If

        '		'UPGRADE_ISSUE: Printer メソッド Printer.EndDoc はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.EndDoc()
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentX = mLeft
        '		'UPGRADE_ISSUE: Printer プロパティ Printer.CurrentY はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"' をクリックしてください。
        '		Printer.CurrentY = mTop

        '		fInit = False
	End Sub
	
	'-------------------------------------------------
	'-------------------------------------------------
End Module