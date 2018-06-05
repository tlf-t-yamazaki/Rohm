Option Strict Off
Option Explicit On
Module basResistorGraph
    '' '' ''	'=========================================================================
    '' '' ''	'   Project Name    |   TKY CHIP - SL432HW
    '' '' ''	'   File Name       |   basResistorGraph.BAS
    '' '' ''	'=========================================================================

    '' '' ''	Private plBackgroundColor As Integer
    '' '' ''	Private plGraphBarColor As Integer
    '' '' ''	Private piLeftMargin As Integer
    '' '' ''	Private pfResistOhmErrorRatio(512) As Double

    '' '' ''	Private Const plGraphBarWidth As Short = 8
    '' '' ''	Private Const plGraphBarStep As Short = 8

    '' '' ''	' 抵抗測定値の設定(トリミングモード時)
    '' '' ''	Public Sub ResistorGraphDataSet2(ByVal xmode As Short)

    '' '' ''		Dim iRegCnt As Short
    '' '' ''		Dim iRegCnt2 As Short
    '' '' ''		Dim i As Short
    '' '' ''		Dim fCutStartX As Double
    '' '' ''		Dim fCutStartY As Double
    '' '' ''		Dim n As Short
    '' '' ''		Dim nn As Short

    '' '' ''		iRegCnt = 0 ' 有効な抵抗数

    '' '' ''		Select Case xmode
    '' '' ''			Case 0
    '' '' ''				For i = 1 To gRegistorCnt
    '' '' ''					If typResistorInfoArray(i).intResNo < 1000 Then
    '' '' ''						iRegCnt = iRegCnt + 1
    '' '' ''						giMeasureResiNum(iRegCnt) = typResistorInfoArray(i).intResNo
    '' '' ''						Select Case gwTrimResult(i)
    '' '' ''							Case TRIM_RESULT_OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case TRIM_RESULT_IT_NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_FT_NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_IT_HING ' IT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_IT_LONG ' IT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_FT_HING ' FT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_FT_LONG ' FT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_OVERRANGE ' Over Range
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 11 ' 4Terminal
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 12 ' IT TEST OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case Else ' N/A, SKIP
    '' '' ''								gfMeasureResiOhm(iRegCnt) = 0#
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcNA
    '' '' ''						End Select
    '' '' ''					End If
    '' '' ''				Next 
    '' '' ''			Case 1
    '' '' ''				For i = 1 To gRegistorCnt
    '' '' ''					If Val(CStr(typResistorInfoArray(i).intResNo)) < 1000 Then
    '' '' ''						iRegCnt = iRegCnt + 1
    '' '' ''						giMeasureResiNum(iRegCnt) = typResistorInfoArray(i).intResNo
    '' '' ''						Select Case gwTrimResult(i)
    '' '' ''							Case TRIM_RESULT_OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case TRIM_RESULT_IT_NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_IT_HING ' IT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_IT_LONG ' IT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_FT_HING ' FT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_FT_LONG ' FT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_OVERRANGE ' Over Range
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 11 ' 4Terminal
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 12 ' IT TEST OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case Else ' N/A, SKIP
    '' '' ''								gfMeasureResiOhm(iRegCnt) = 0#
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcNA
    '' '' ''						End Select
    '' '' ''					End If
    '' '' ''				Next 
    '' '' ''			Case 2
    '' '' ''				For i = 1 To gRegistorCnt
    '' '' ''					If Val(CStr(typResistorInfoArray(i).intResNo)) < 1000 Then
    '' '' ''						iRegCnt = iRegCnt + 1
    '' '' ''						giMeasureResiNum(iRegCnt) = Val(CStr(typResistorInfoArray(i).intResNo))
    '' '' ''						Select Case gwTrimResult(i)
    '' '' ''							Case TRIM_RESULT_OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case TRIM_RESULT_IT_NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_IT_HING ' IT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_IT_LONG ' IT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case TRIM_RESULT_FT_HING ' FT HI NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_FT_LONG ' FT LO NG
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcFT
    '' '' ''							Case TRIM_RESULT_OVERRANGE ' Over Range
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 11 ' 4Terminal
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfFinalTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcIT
    '' '' ''							Case 12 ' IT TEST OK
    '' '' ''								gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''							Case Else ' N/A, SKIP
    '' '' ''								gfMeasureResiOhm(iRegCnt) = 0#
    '' '' ''								giMeasureResiRst(iRegCnt) = cMEASUREcNA
    '' '' ''						End Select
    '' '' ''					End If
    '' '' ''				Next 
    '' '' ''			Case 3
    '' '' ''				For i = 1 To gRegistorCnt
    '' '' ''					If Val(CStr(typResistorInfoArray(i).intResNo)) < 1000 Then
    '' '' ''						iRegCnt = iRegCnt + 1
    '' '' ''						giMeasureResiNum(iRegCnt) = Val(CStr(typResistorInfoArray(i).intResNo))
    '' '' ''						gfMeasureResiOhm(iRegCnt) = gfInitialTest(i)
    '' '' ''						giMeasureResiRst(iRegCnt) = cMEASUREcOK
    '' '' ''					End If
    '' '' ''				Next 
    '' '' ''		End Select

    '' '' ''		iRegCnt2 = 0
    '' '' ''		For i = 1 To gRegistorCnt
    '' '' ''			If Val(CStr(typResistorInfoArray(i).intResNo)) < 1000 Then
    '' '' ''				iRegCnt2 = iRegCnt2 + 1

    '' '' ''                If typResistorInfoArray(i).intTargetValType = TARGET_TYPE_ABSOLUTE Then
    '' '' ''                    gfResistorTarget(iRegCnt2) = typResistorInfoArray(i).dblTrimTargetVal
    '' '' ''                Else
    '' '' ''                    gfResistorTarget(iRegCnt2) = gfTargetVal(i)
    '' '' ''                End If

    '' '' ''				Call GetResistorCutAddress(i, 1, n, nn)
    '' '' ''                fCutStartX = CDbl(typResistorInfoArray(n).ArrCut(nn).dblStartPointX) ' Cut starting point X
    '' '' ''                fCutStartY = CDbl(typResistorInfoArray(n).ArrCut(nn).dblStartPointY) ' Cut starting point Y
    '' '' ''				gfMeasureResiPos(1, iRegCnt2) = fCutStartX
    '' '' ''				gfMeasureResiPos(2, iRegCnt2) = fCutStartY
    '' '' ''			End If
    '' '' ''		Next 
    '' '' ''		If iRegCnt <> iRegCnt2 Then
    '' '' ''			Stop
    '' '' ''		End If

    '' '' ''		giMeasureResistors = iRegCnt

    '' '' ''	End Sub

    '' '' ''	Public Sub ResistorGraphVisible(ByVal flag As Boolean)

    '' '' ''		Dim r As Short
    '' '' ''		If (flag = True) Then
    '' '' ''			If typPlateInfo.intMeasType <> 0 Then
    '' '' ''				Exit Sub
    '' '' ''			End If

    '' '' ''            'Form1.frmGraph.Visible = True
    '' '' ''            'Form1.picGraphView.Visible = True
    '' '' ''            'Form1.picVirticalLine.Visible = True
    '' '' ''            'Form1.FlatScrollBar1.Visible = True
    '' '' ''            'Form1.FlatScrollBar1.Value = 1

    '' '' ''			r = ResistorGraphDraw()
    '' '' ''			If r Then
    '' '' ''                'Form1.frmGraph.Visible = False
    '' '' ''                'Form1.picGraphView.Visible = False
    '' '' ''                'Form1.picVirticalLine.Visible = False
    '' '' ''                'Form1.FlatScrollBar1.Visible = False
    '' '' ''                'Form1.StatusBar.Visible = False

    '' '' ''				'UPGRADE_ISSUE: PictureBox メソッド picGraphView.Cls はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''                'Form1.picGraphView.Cls()
    '' '' ''                'Form1.picGraphView.Width = VB6.TwipsToPixelsX(640)
    '' '' ''			End If

    '' '' ''            'Form1.StatusBar.Text = ""
    '' '' ''		Else
    '' '' ''            'Form1.frmGraph.Visible = False
    '' '' ''            'Form1.picGraphView.Visible = False
    '' '' ''            'Form1.picVirticalLine.Visible = False
    '' '' ''            'Form1.FlatScrollBar1.Visible = False
    '' '' ''            'Form1.StatusBar.Visible = False

    '' '' ''			'UPGRADE_ISSUE: PictureBox メソッド picGraphView.Cls はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''            'Form1.picGraphView.Cls()
    '' '' ''            'Form1.picGraphView.Width = VB6.TwipsToPixelsX(640)
    '' '' ''		End If

    '' '' ''	End Sub

    '' '' ''	Private Function ResistorGraphDraw() As Short
    '' '' ''		Dim BF As Object

    '' '' ''		Dim pCtr As Short
    '' '' ''		Dim ScaleDistance As Short
    '' '' ''		Dim ScaleNum As Double
    '' '' ''		Dim plX_AxisEndPoint As Integer
    '' '' ''		Dim frm As System.Windows.Forms.Form
    '' '' ''		Dim x As Short
    '' '' ''		Dim y As Short
    '' '' ''		Dim fMax As Double
    '' '' ''		Dim fMin As Double
    '' '' ''		Dim c As Integer
    '' '' ''		Dim s As String
    '' '' ''		Dim fScaleY As Double
    '' '' ''		Dim iScaleYcnt As Short
    '' '' ''		Dim frate As Double
    '' '' ''		Dim n As Integer
    '' '' ''		Dim igHalfHeight As Integer
    '' '' ''		Dim igCenterY As Integer
    '' '' ''		Dim igBaseY As Integer
    '' '' ''		Dim sFormat As String
    '' '' ''		Dim igScaleYPixels As Integer
    '' '' ''		Dim iTextHeight As Integer
    '' '' ''		Dim iTextWidth As Integer

    '' '' ''		On Error Resume Next

    '' '' ''		ResistorGraphDraw = 0

    '' '' ''		plBackgroundColor = &HC0FFFF '&H40C0C0    '&H8000000F
    '' '' ''		plGraphBarColor = &HFF8080 '&HC0C0F0

    '' '' ''		frm = Form1

    '' '' ''        ''UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'frm.picGraphView.Cls()
    '' '' ''        ''UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        'frm.picGraphView.ScaleMode = vbPixels
    '' '' ''        ''UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        'frm.picVirticalLine.ScaleMode = vbPixels
    '' '' ''        ''UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'iTextHeight = frm.picVirticalLine.TextHeight("0")
    '' '' ''        ''UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'iTextWidth = frm.picVirticalLine.TextWidth("0")
    '' '' ''        ''UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'piLeftMargin = frm.picVirticalLine.ScaleWidth
    '' '' ''		igCenterY = 63
    '' '' ''		igHalfHeight = igCenterY - iTextHeight / 2
    '' '' ''		igBaseY = 125
    '' '' ''        ''UPGRADE_ISSUE: Control frmGraph は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: Control Label5 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'frm.frmGraph.Left = frm.Label5.Left
    '' '' ''        ''UPGRADE_ISSUE: Control frmGraph は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: Control Label5 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'frm.frmGraph.Top = frm.Label5.Top
    '' '' ''        ''UPGRADE_ISSUE: Control frmGraph は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: Control Label5 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'frm.frmGraph.width = frm.Label5.width
    '' '' ''        ''UPGRADE_ISSUE: Control frmGraph は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        ''UPGRADE_ISSUE: Control Label5 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'frm.frmGraph.Height = frm.Label5.Height

    '' '' ''		For pCtr = 1 To giMeasureResistors
    '' '' ''			pfResistOhmErrorRatio(pCtr) = 0
    '' '' ''			If gfMeasureResiOhm(pCtr) <= gSysPrm.stMES.gdRESISTOR_MAX And gfResistorTarget(pCtr) > 0# Then ' @@@217
    '' '' ''				On Error Resume Next
    '' '' ''				pfResistOhmErrorRatio(pCtr) = gfMeasureResiOhm(pCtr) / gfResistorTarget(pCtr) * 100# - 100#

    '' '' ''				If gfMeasureResiOhm(pCtr) > 9999.9999 Then
    '' '' ''					gfMeasureResiOhm(pCtr) = 9999.9999
    '' '' ''				End If

    '' '' ''				On Error GoTo 0
    '' '' ''			Else
    '' '' ''				pfResistOhmErrorRatio(pCtr) = 0
    '' '' ''			End If
    '' '' ''		Next 

    '' '' ''        'On Error GoTo ErrorHandler

    '' '' ''		'最大、最小値を探す
    '' '' ''		fMax = 0# : fMin = 100000#
    '' '' ''		For pCtr = 1 To giMeasureResistors
    '' '' ''			If fMax < pfResistOhmErrorRatio(pCtr) Then
    '' '' ''				fMax = pfResistOhmErrorRatio(pCtr)
    '' '' ''			End If
    '' '' ''			If fMin > pfResistOhmErrorRatio(pCtr) Then
    '' '' ''				fMin = pfResistOhmErrorRatio(pCtr)
    '' '' ''			End If
    '' '' ''		Next pCtr
    '' '' ''		Debug.Print("MAX=" & fMax & ", MIN=" & fMin)
    '' '' ''		' 絶対値が大きいほうをとる
    '' '' ''		fMin = System.Math.Abs(fMin)
    '' '' ''		fMax = System.Math.Abs(fMax)
    '' '' ''		If fMin > fMax Then
    '' '' ''			fMax = System.Math.Abs(fMin)
    '' '' ''		End If

    '' '' ''		'縦軸のスケールを決める
    '' '' ''		frate = fMax
    '' '' ''		If frate <= 0.001 Then
    '' '' ''			fScaleY = 0.001
    '' '' ''			iScaleYcnt = 1
    '' '' ''			sFormat = "0.000"
    '' '' ''		ElseIf frate <= 0.005 Then 
    '' '' ''			fScaleY = 0.001
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.000"
    '' '' ''		ElseIf frate <= 0.01 Then 
    '' '' ''			fScaleY = 0.002
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.000"
    '' '' ''		ElseIf frate <= 0.05 Then 
    '' '' ''			fScaleY = 0.01
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.00"
    '' '' ''		ElseIf frate <= 0.1 Then 
    '' '' ''			fScaleY = 0.02
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.00"
    '' '' ''		ElseIf frate <= 0.5 Then 
    '' '' ''			fScaleY = 0.1
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.0"
    '' '' ''		ElseIf frate <= 1# Then 
    '' '' ''			fScaleY = 0.2
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0.0"
    '' '' ''		ElseIf frate <= 5# Then 
    '' '' ''			fScaleY = 1#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 10# Then 
    '' '' ''			fScaleY = 2#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 50# Then 
    '' '' ''			fScaleY = 10#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 100# Then 
    '' '' ''			fScaleY = 20#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 500# Then 
    '' '' ''			fScaleY = 100#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 1000# Then 
    '' '' ''			fScaleY = 200#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		ElseIf frate <= 5000# Then 
    '' '' ''			fScaleY = 1000#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		Else
    '' '' ''			fScaleY = 1000#
    '' '' ''			iScaleYcnt = frate / fScaleY
    '' '' ''			If fScaleY * iScaleYcnt < frate Then
    '' '' ''				iScaleYcnt = iScaleYcnt + 1
    '' '' ''			End If
    '' '' ''			sFormat = "0"
    '' '' ''		End If


    '' '' ''        'With frm
    '' '' ''        '背景色
    '' '' ''        ''UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '.picVirticalLine.BackColor = plBackgroundColor
    '' '' ''        ''UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '.picGraphView.BackColor = plBackgroundColor

    '' '' ''        ScaleNum = 0
    '' '' ''        ScaleDistance = 0
    '' '' ''        ''UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '.picGraphView.CurrentX = 0
    '' '' ''        ''UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '.picGraphView.CurrentY = 0

    '' '' ''        'ｘ-ｙ軸補助線を引く,スクロールバーの設定も行う

    '' '' ''        plX_AxisEndPoint = piLeftMargin + (giMeasureResistors + 1) * (plGraphBarStep + plGraphBarWidth)

    '' '' ''        If plX_AxisEndPoint < 640 Then
    '' '' ''            plX_AxisEndPoint = 640
    '' '' ''            'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''            '.FlatScrollBar1.Enabled = False
    '' '' ''        Else
    '' '' ''            'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''            '.FlatScrollBar1.Enabled = True
    '' '' ''        End If

    '' '' ''        ' 抵抗数に合わせてグラフの picturebox の幅を変更する
    '' '' ''        'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        'UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        'UPGRADE_ISSUE: Form メソッド frm.ScaleX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '.picGraphView.width = frm.ScaleX(plX_AxisEndPoint, vbPixels, vbTwips)

    '' '' ''        'ｘ軸目数字
    '' '' ''        'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '.picGraphView.CurrentY = igBaseY
    '' '' ''        'For pCtr = 1 To giMeasureResistors
    '' '' ''        '	If pCtr = 1 Or (pCtr Mod 5) = 0 Then
    '' '' ''        '		x = pCtr * (plGraphBarStep + plGraphBarWidth) + piLeftMargin - plGraphBarWidth
    '' '' ''        '		'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picGraphView.CurrentX = x
    '' '' ''        '		'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picGraphView.CurrentY = igBaseY + 5
    '' '' ''        '		s = pCtr.ToString("0")
    '' '' ''        '		'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picGraphView.Print(s)
    '' '' ''        '	End If
    '' '' ''        'Next pCtr

    '' '' ''        'X軸目盛り
    '' '' ''        'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'UPGRADE_ISSUE: 定数 vbDot はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '.picGraphView.DrawStyle = vbDot 'vbSolid
    '' '' ''        'For pCtr = 1 To giMeasureResistors
    '' '' ''        '	' igBaseY - 2)-
    '' '' ''        '	If (pCtr Mod 10) = 0 Then
    '' '' ''        '		c = &H8080FF
    '' '' ''        '	Else
    '' '' ''        '		c = &HFFFFFF
    '' '' ''        '	End If
    '' '' ''        '	x = piLeftMargin + pCtr * (plGraphBarWidth + plGraphBarStep) - plGraphBarStep / 2
    '' '' ''        '	'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.picGraphView.Line((x, 0) - (x, igBaseY + 2), c)
    '' '' ''        'Next 
    '' '' ''        'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        'UPGRADE_ISSUE: 定数 vbSolid はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	.picGraphView.DrawStyle = vbSolid


    '' '' ''        '	igScaleYPixels = igHalfHeight / iScaleYcnt

    '' '' ''        '	' X軸
    '' '' ''        '	'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.picGraphView.Line((piLeftMargin, igBaseY) - (plX_AxisEndPoint, igBaseY), &H0s)
    '' '' ''        '	' Y軸
    '' '' ''        '	'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.picVirticalLine.Line((piLeftMargin - 1, 1) - (piLeftMargin - 1, igBaseY), &H0s)
    '' '' ''        '	' センター
    '' '' ''        '	'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.picGraphView.Line((piLeftMargin, igCenterY) - (plX_AxisEndPoint, igCenterY), &H0s)

    '' '' ''        '	' ｙ軸に目盛りを振る
    '' '' ''        '	For pCtr = -iScaleYcnt To iScaleYcnt
    '' '' ''        '		y = igCenterY - pCtr * igScaleYPixels

    '' '' ''        '		If pCtr <> 0 Then
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			'UPGRADE_ISSUE: 定数 vbDot はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '			.picGraphView.DrawStyle = vbDot
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.Line((0, y) - (plX_AxisEndPoint, y), &H0s)
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			'UPGRADE_ISSUE: 定数 vbSolid はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '			.picGraphView.DrawStyle = vbSolid
    '' '' ''        '		End If

    '' '' ''        '		ScaleNum = fScaleY * pCtr
    '' '' ''        '		s = ScaleNum.ToString(sFormat) & "%"
    '' '' ''        '		If Left(s, 1) <> "-" Then s = " " & s

    '' '' ''        '		'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		x = .picVirticalLine.ScaleWidth
    '' '' ''        '		'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		x = x - .picVirticalLine.TextWidth(s) - iTextWidth / 2
    '' '' ''        '		'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picVirticalLine.CurrentX = x
    '' '' ''        '		y = y - iTextHeight / 2
    '' '' ''        '		'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picVirticalLine.CurrentY = y

    '' '' ''        '		'UPGRADE_ISSUE: Control picVirticalLine は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '		.picVirticalLine.Print(s)

    '' '' ''        '	Next pCtr


    '' '' ''        '	'棒グラフの出力
    '' '' ''        '	'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.picGraphView.CurrentX = piLeftMargin
    '' '' ''        '	For pCtr = 1 To giMeasureResistors
    '' '' ''        '		If gfMeasureResiOhm(pCtr) <= gSysPrm.stMES.gdRESISTOR_MAX Then ' @@@217
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.CurrentY = igCenterY
    '' '' ''        '			y = (igScaleYPixels * iScaleYcnt) * (pfResistOhmErrorRatio(pCtr) / (iScaleYcnt * fScaleY))
    '' '' ''        '			y = y * -1#
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.CurrentX = piLeftMargin + ((plGraphBarStep + plGraphBarWidth) * (pCtr)) - plGraphBarWidth
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.Line((plGraphBarWidth, y), plGraphBarColor, BF) ' vbBlack
    '' '' ''        '		Else
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			'UPGRADE_ISSUE: Form メソッド frm.TextHeight はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '			.picGraphView.CurrentY = igCenterY - .TextHeight("X") / 2
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.CurrentX = piLeftMargin + ((plGraphBarStep + plGraphBarWidth) * (pCtr)) - plGraphBarWidth
    '' '' ''        '			'UPGRADE_ISSUE: Control picGraphView は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '			.picGraphView.Print("X")
    '' '' ''        '		End If
    '' '' ''        '	Next pCtr

    '' '' ''        '	' スクロールバーの設定
    '' '' ''        '	' コンテナコントロール(frame)に対してのpictureboxの位置大きさはTWIPS単位
    '' '' ''        '	' なので、スケールを変換して設定する

    '' '' ''        '	'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: Form メソッド frm.ScaleX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '	.FlatScrollBar1.width = frm.ScaleX(640, vbPixels, vbTwips)

    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: Form メソッド frm.ScaleX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '	n = frm.ScaleX(1, vbPixels, vbTwips)
    '' '' ''        '	'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.FlatScrollBar1.SmallChange = n

    '' '' ''        '	n = (640 - piLeftMargin) / (plGraphBarWidth + plGraphBarStep)
    '' '' ''        '	If n < 1 Then n = 1
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: Form メソッド frm.ScaleX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '	n = frm.ScaleX(n, vbPixels, vbTwips)
    '' '' ''        '	'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.FlatScrollBar1.LargeChange = n

    '' '' ''        '	'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	.FlatScrollBar1.min = 1
    '' '' ''        '	If plX_AxisEndPoint > 640 Then
    '' '' ''        '		plX_AxisEndPoint = plX_AxisEndPoint - 640
    '' '' ''        '	End If
    '' '' ''        '	x = plX_AxisEndPoint / (plGraphBarWidth + plGraphBarStep)
    '' '' ''        '	'UPGRADE_ISSUE: Control FlatScrollBar1 は、汎用名前空間 Form 内にあるため、解決できませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="084D22AD-ECB1-400F-B4C7-418ECEC5E36E"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbTwips はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: 定数 vbPixels はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"' をクリックしてください。
    '' '' ''        '	'UPGRADE_ISSUE: Form メソッド frm.ScaleX はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        '	.FlatScrollBar1.MAX = frm.ScaleX(x, vbPixels, vbTwips) 'plX_AxisEndPoint

    '' '' ''        'End With

    '' '' ''        Exit Function

    '' '' ''ErrorHandler:
    '' '' ''        Resume ErrorHandler1
    '' '' ''ErrorHandler1:
    '' '' ''        Dim er As Short
    '' '' ''        er = Err.Number
    '' '' ''        On Error GoTo 0
    '' '' ''        ResistorGraphDraw = 1
    '' '' ''        MsgBox("Internal error X004-" & Str(er), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, gAppName)
    '' '' ''    End Function

    '' '' ''	Public Sub ResistorGraphClick(ByRef pic As System.Windows.Forms.PictureBox, ByRef Button As Short, ByRef Shift As Short, ByRef x As Single, ByRef y As Single)

    '' '' ''		Dim cx As Integer
    '' '' ''		Dim cxLeft As Integer
    '' '' ''		Dim cxRight As Integer
    '' '' ''		Dim xbp As Double
    '' '' ''		Dim ybp As Double

    '' '' ''		cx = (x - piLeftMargin) / (plGraphBarStep + plGraphBarWidth)
    '' '' ''		If cx < 1 Or cx > giMeasureResistors Then Exit Sub

    '' '' ''		cxRight = piLeftMargin + cx * (plGraphBarStep + plGraphBarWidth)
    '' '' ''		cxLeft = cxRight - plGraphBarWidth + 1

    '' '' ''		If x >= cxLeft And x <= cxRight Then

    '' '' ''            'giResistorNumber = giMeasureResiNum(cx) ' 選択した抵抗番号

    '' '' ''			' HI選択時
    '' '' ''            'If (gTkyKnd = KND_TKY) Then
    '' '' ''            '    'UPGRADE_NOTE: 式 TKY_MODULE が True に評価されなかったか、またはまったく評価されなかったため、#If #EndIf ブロックはアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="27EE2C3C-05AF-4C04-B2AF-657B4FB6B5FC"' をクリックしてください。
    '' '' ''            '    ''''2009/07/08 minato
    '' '' ''            '    '''    プローブOCXからパラメータを取得する
    '' '' ''            '    If Form1.opBPMode(OPTION_BPMODE_HI).Value = True Then
    '' '' ''            '        xbp = gPbTeachDataHI(giResistorNumber, 1)
    '' '' ''            '        ybp = gPbTeachDataHI(giResistorNumber, 2)
    '' '' ''            '        ' LO選択時
    '' '' ''            '    ElseIf Form1.opBPMode(OPTION_BPMODE_LO).Value = True Then
    '' '' ''            '        xbp = gPbTeachDataLO(giResistorNumber, 1)
    '' '' ''            '        ybp = gPbTeachDataLO(giResistorNumber, 2)
    '' '' ''            '    Else
    '' '' ''            '        xbp = gfMeasureResiPos(1, cx)
    '' '' ''            '        ybp = gfMeasureResiPos(2, cx)
    '' '' ''            '    End If
    '' '' ''            'Else
    '' '' ''            '    xbp = gfMeasureResiPos(1, cx)
    '' '' ''            '    ybp = gfMeasureResiPos(2, cx)
    '' '' ''            'End If

    '' '' ''            'gGraphClick = True
    '' '' ''            'gGraphClickBpx = xbp
    '' '' ''            'gGraphClickBpy = ybp

    '' '' ''            '        Call BpMove(xbp, ybp, 1)
    '' '' ''            Call Form1.System1.EX_MOVE(gSysPrm, xbp, ybp, 1)

    '' '' ''            If giAppMode = APP_MODE_PROBE Then
    '' '' ''                'Call ProbeTeachBpDisp()
    '' '' ''            End If
    '' '' ''        End If

    '' '' ''	End Sub

    '' '' ''	Public Sub ResistorGraphMouseMove(ByRef pic As System.Windows.Forms.PictureBox, ByRef Button As Short, ByRef Shift As Short, ByRef x As Single, ByRef y As Single)

    '' '' ''		Dim cx As Integer
    '' '' ''		Dim cxLeft As Integer
    '' '' ''		Dim cxRight As Integer
    '' '' ''		Dim s As String

    '' '' ''		cx = (x - piLeftMargin) / (plGraphBarStep + plGraphBarWidth)
    '' '' ''		If cx < 1 Or cx > giMeasureResistors Then
    '' '' ''			'UPGRADE_ISSUE: PictureBox プロパティ pic.ToolTipText はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''            'pic.ToolTipText = ""
    '' '' ''            'Form1.StatusBar.Text = ""
    '' '' ''			Exit Sub
    '' '' ''		End If

    '' '' ''		cxRight = piLeftMargin + cx * (plGraphBarStep + plGraphBarWidth)
    '' '' ''		cxLeft = cxRight - plGraphBarWidth + 1

    '' '' ''		If x >= cxLeft And x <= cxRight Then
    '' '' ''            s = "No." & CStr(giMeasureResiNum(cx)) & ",  " & "Target=" & gfResistorTarget(cx).ToString("0.0000") & "(ohm),  " & "Measure=" & gfMeasureResiOhm(cx).ToString("0.0000") & "(ohm),  " & pfResistOhmErrorRatio(cx).ToString("0.0000") & "% "
    '' '' ''		Else
    '' '' ''			s = ""
    '' '' ''		End If

    '' '' ''		'UPGRADE_ISSUE: PictureBox プロパティ pic.ToolTipText はアップグレードされませんでした。 詳細については、'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"' をクリックしてください。
    '' '' ''        'pic.ToolTipText = s
    '' '' ''        'Form1.StatusBar.Text = s

    '' '' ''	End Sub


    '' '' ''	Public Sub graphScrollChange()

    '' '' ''        'Dim x As Integer
    '' '' ''        '' グラフの横スクロール

    '' '' ''        'x = plGraphBarWidth + plGraphBarStep
    '' '' ''        'x = -(Form1.FlatScrollBar1.Value) * x
    '' '' ''        'Call Form1.picGraphView.SetBounds(VB6.TwipsToPixelsX(x), 0, 0, 0, Windows.Forms.BoundsSpecified.X)

    '' '' ''	End Sub
End Module