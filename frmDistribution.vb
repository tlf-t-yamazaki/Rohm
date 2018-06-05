'===============================================================================
'   Description  : ���z�}�\������
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class frmDistribution
	Inherits System.Windows.Forms.Form
#Region "�v���C�x�[�g�萔��`"
    '===========================================================================
    '   �萔��`
    '===========================================================================
    ''----- ��ʺ�� �----
    '' ����۰����ڰĂ���
    'Private Declare Sub keybd_event Lib "user32.dll" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)
    'Private Const VK_SNAPSHOT As Short = &H2CS          ' PrtSc key
    'Private Const VK_LMENU As Short = &HA4S             ' Alt key
    'Private Const KEYEVENTF_KEYUP As Short = &H2S       ' ����UP���
    'Private Const KEYEVENTF_EXTENDEDKEY As Short = &H1S ' ���݂͊g������

    ' ��ʕ\���ʒu�I�t�Z�b�g
    'Private Const DISP_X_OFFSET As Integer = 4                         '###065
    'Private Const DISP_Y_OFFSET As Integer = 20                        '###065
    Private Const DISP_X_OFFSET As Integer = 0                          '###065
    Private Const DISP_Y_OFFSET As Integer = 0                          '###065

#End Region

#Region "�����o�ϐ���`"
    '===========================================================================
    '   �����o�ϐ���`
    '===========================================================================
    Private m_bInitDistForm As Boolean
    Private m_bFgDispGrp As Boolean                                ' �\�����̎��(TRUE:IT FALSE:FT)
#End Region

#Region "�t�H�[��������"
    '''=========================================================================
    '''<summary>̫�я�����������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitializeForm()
        Dim strMSG As String

        Try
            ' ���z�}�\���p���x���z��̏�����
            gDistRegNumLblAry(0) = Me.LblRegN_00             ' ���z�O���t��R���z��(0�`11)
            gDistRegNumLblAry(1) = Me.LblRegN_01
            gDistRegNumLblAry(2) = Me.LblRegN_02
            gDistRegNumLblAry(3) = Me.LblRegN_03
            gDistRegNumLblAry(4) = Me.LblRegN_04
            gDistRegNumLblAry(5) = Me.LblRegN_05
            gDistRegNumLblAry(6) = Me.LblRegN_06
            gDistRegNumLblAry(7) = Me.LblRegN_07
            gDistRegNumLblAry(8) = Me.LblRegN_08
            gDistRegNumLblAry(9) = Me.LblRegN_09
            gDistRegNumLblAry(10) = Me.LblRegN_10
            gDistRegNumLblAry(11) = Me.LblRegN_11

            gDistGrpPerLblAry(0) = Me.LblGrpPer_00           ' ���z�O���t%�z��(0�`11)
            gDistGrpPerLblAry(1) = Me.LblGrpPer_01
            gDistGrpPerLblAry(2) = Me.LblGrpPer_02
            gDistGrpPerLblAry(3) = Me.LblGrpPer_03
            gDistGrpPerLblAry(4) = Me.LblGrpPer_04
            gDistGrpPerLblAry(5) = Me.LblGrpPer_05
            gDistGrpPerLblAry(6) = Me.LblGrpPer_06
            gDistGrpPerLblAry(7) = Me.LblGrpPer_07
            gDistGrpPerLblAry(8) = Me.LblGrpPer_08
            gDistGrpPerLblAry(9) = Me.LblGrpPer_09
            gDistGrpPerLblAry(10) = Me.LblGrpPer_10
            gDistGrpPerLblAry(11) = Me.LblGrpPer_11

            gDistShpGrpLblAry(0) = Me.LblShpGrp_00                      ' ���z�O���t�z��(0�`11)
            gDistShpGrpLblAry(1) = Me.LblShpGrp_01
            gDistShpGrpLblAry(2) = Me.LblShpGrp_02
            gDistShpGrpLblAry(3) = Me.LblShpGrp_03
            gDistShpGrpLblAry(4) = Me.LblShpGrp_04
            gDistShpGrpLblAry(5) = Me.LblShpGrp_05
            gDistShpGrpLblAry(6) = Me.LblShpGrp_06
            gDistShpGrpLblAry(7) = Me.LblShpGrp_07
            gDistShpGrpLblAry(8) = Me.LblShpGrp_08
            gDistShpGrpLblAry(9) = Me.LblShpGrp_09
            gDistShpGrpLblAry(10) = Me.LblShpGrp_10
            gDistShpGrpLblAry(11) = Me.LblShpGrp_11

            'V4.0.0.0�K��
            gGoodChip = lblGoodChip
            gNgChip = lblNgChip
            gMaxValue = lblMaxValue
            gMinValue = lblMinValue
            gAverageValue = lblAverageValue
            gDeviationValue = lblDeviationValue
            gGraphAccumulationTitle = lblGraphAccumulationTitle
            gRegistUnit = lblRegistUnit
            'V4.0.0.0�K��

            'DistRegItLblAry(i) = New System.Windows.Forms.Label     ' ���z�O���t��R��(IT)�z��
            'DistRegFtLblAry(i) = New System.Windows.Forms.Label     ' ���z�O���t��R��(FT)�z��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "frmDistribution.InitializeForm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�C�j�V����/�t�@�C�i�����z�}�̕\�����"
    Public Function DisplayInitialMode() As Boolean
        Return m_bFgDispGrp
    End Function
#End Region

#Region "���z�}�ۑ��{�^������������"
    '''=========================================================================
    '''<summary>���z�}�ۑ��{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdGraphSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdGraphSave.Click

        ' �{�^������
        cmdGraphSave.Enabled = False
        cmdInitial.Enabled = False
        cmdFinal.Enabled = False

        ' ��ʂ��n�[�h�R�s�[���������
        Call SaveWindowPic(True, False)

        ' �������b�Z�[�W

        ' �{�^������
        cmdGraphSave.Enabled = True
        cmdInitial.Enabled = True
        cmdFinal.Enabled = True

    End Sub
#End Region

#Region "���z�}�ۑ�����"
    '''=========================================================================
    '''<summary>���z�}�ۑ��{�^������������</summary>
    '''<remarks>PrintScreen�L�[�������Ɠ����̏������s��</remarks>
    '''=========================================================================
    Private Sub SaveWindowPic(Optional ByRef ActWind As Boolean = True, Optional ByRef PrintOn As Boolean = False)

        Dim msg As String               'V4.7.0.0�B

        Try
            If (String.IsNullOrEmpty(typPlateInfo.strDataName)) Then Exit Sub 'V4.7.0.0�B

            Dim fileName As String
            Dim bFileSave As Boolean
            Dim bitMap As New Bitmap(Me.Width, Me.Height)
            bFileSave = False
            fileName = ""

            ''�A�N�e�B�u��Window���N���b�v�{�[�h�փR�s�[
            'SendKeys.SendWait("%{PRTSC}")

            '' �N���b�v�{�[�h����f�[�^�擾
            'Dim obj As IDataObject = Clipboard.GetDataObject()

            'If obj IsNot Nothing Then
            '    Dim dispImage As Image = DirectCast(obj.GetData(DataFormats.Bitmap), Image)

            '    If dispImage IsNot Nothing Then
            '        If m_bFgDispGrp = True Then
            '            fileName = gSysPrm.stLOG.gsLoggingDir & "IT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            '        Else
            '            fileName = gSysPrm.stLOG.gsLoggingDir & "FT_MAP" & Now.ToString("yyMMddhhmmss") & ".BMP"
            '        End If

            '        dispImage.Save(fileName)
            '        bFileSave = True
            '    End If
            'End If

            ' �د���ް�ނ�÷��(Bitmap�ȊO�H)����߰����Ă����Ԃ���
            ' dispImage��Nothing�ƂȂ��ĕۑ�����Ȃ����ߕύX              'V4.7.0.0�B
            Dim ITFT As String
            If (True = m_bFgDispGrp) Then
                ITFT = "_IT_MAP"
            Else
                ITFT = "_FT_MAP"
            End If

            fileName = gSysPrm.stLOG.gsLoggingDir & _
                IO.Path.GetFileNameWithoutExtension(typPlateInfo.strDataName) & _
                ITFT & Now.ToString("yyMMddHHmmss") & ".BMP"

            Using bmp As New Bitmap(Me.Width, Me.Height)
                Me.DrawToBitmap(bmp, New Rectangle(0, 0, Me.Width, Me.Height))
                bmp.Save(fileName)
                bFileSave = True
            End Using

            '���ʂ̕\��
            If (bFileSave = True) Then
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�ۑ������I" & vbCrLf & " (" & fileName & ")")
                'Else
                '    MsgBox("Save completion." & vbCrLf & " (" & fileName & ")")
                'End If
                msg = frmDistribution_001 & vbCrLf & " (" & fileName & ")"
            Else
                'If gSysPrm.stTMN.giMsgTyp = 0 Then
                '    MsgBox("�ۑ��ł��܂���ł����B")
                'Else
                '    MsgBox("I was not able to save it.")
                'End If
                msg = frmDistribution_002
            End If

            'V6.0.2.0�B�@Exit Sub

        Catch ex As Exception
            'If gSysPrm.stTMN.giMsgTyp = 0 Then
            '    MsgBox("�ۑ��ł��܂���ł����B")
            'Else
            '    MsgBox("I was not able to save it.")
            'End If
            msg = frmDistribution_002
        End Try

        ' ���ɉB��Ȃ��悤�ɑΉ�       'V4.7.0.0�B
        MessageBox.Show(msg, cmdGraphSave.Text, MessageBoxButtons.OK, MessageBoxIcon.None,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)

        Me.TopMost = True ''V6.0.2.0�C

    End Sub
#End Region

#Region "�t�@�C�i���e�X�g���z�}�\���{�^������������"
    '''=========================================================================
    '''<summary>�t�@�C�i���e�X�g���z�}�\���{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdFinal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFinal.Click
        m_bFgDispGrp = False
        Call RedrawGraph()                                              ' ���z�}�\������
    End Sub
#End Region

#Region "�C�j�V�����e�X�g���z�}�\���{�^������������"
    '''=========================================================================
    '''<summary>�C�j�V�����e�X�g���z�}�\���{�^������������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub cmdInitial_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdInitial.Click
        m_bFgDispGrp = True
        Call RedrawGraph()
    End Sub
#End Region

#Region "�t�H�[�����[�h������"
    '''=========================================================================
    '''<summary>�t�H�[�����[�h������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        'Dim utdClientPoint As tagPOINT
        'Dim lngWin32apiResultCode As Integer
        Dim setLocation As System.Drawing.Point

        '���������s
        If (m_bInitDistForm = False) Then
            InitializeForm()
            m_bInitDistForm = True
        End If

        'bFgfrmDistribution = True                           ' ���Y���̕\���׸�ON

        'Video�̏�ɕ\������B
        setLocation = Form1.VideoLibrary1.Location
        setLocation.X = setLocation.X + DISP_X_OFFSET
        setLocation.Y = setLocation.Y + DISP_Y_OFFSET
        Me.Location = setLocation

        'lblRegistTitle.Text = PIC_TRIM_09
        'lblGoodTitle.Text = PIC_TRIM_03
        'lblNgTitle.Text = PIC_TRIM_04
        'lblMinTitle.Text = PIC_TRIM_05
        'lblMaxTitle.Text = PIC_TRIM_06
        'lblAverage.Text = PIC_TRIM_07
        'lblDeviation.Text = PIC_TRIM_08
        'cmdInitial.Text = PIC_TRIM_01
        'cmdFinal.Text = PIC_TRIM_02

        ' ���z�}�ޯ�ϯ�ߕۑ�
        cmdGraphSave.Visible = True
        'cmdGraphSave.Text = PIC_TRIM_10
        RedrawGraph()

        '��ɍőO�ʂɕ\������B
        Me.TopMost = True
    End Sub
#End Region

#Region "�t�H�[�J�X�����������̏���"
    '''=========================================================================
    '''<summary>���M���O�J�n(�W��)</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_LostFocus(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.LostFocus
        '    Unload Me
    End Sub
#End Region

#Region "�t�H�[���A�����[�h������"
    '''=========================================================================
    '''<summary>�t�H�[���A�����[�h������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub frmDistribution_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        'bFgfrmDistribution = False                      ' ���Y���̕\���׸�OFF
        Form1.chkDistributeOnOff.Checked = False

        'If (gSysPrm.stTMN.giMsgTyp = 0) Then
        '    Form1.chkDistributeOnOff.Text = "���Y�O���t�@�\��"
        'Else
        '    Form1.chkDistributeOnOff.Text = "Distribute ON"
        'End If
        Form1.chkDistributeOnOff.Text = frmDistribution_003
    End Sub
#End Region

#Region "���z�}�\������"
    '''=========================================================================
    '''<summary>���z�}�\������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Public Sub RedrawGraph()

        Dim iCnt As Short                                   ' ����
        Dim lMax As Integer
        Dim lScale As Integer
        Dim lScaleMax As Integer
        Dim dblGraphDiv As Double
        Dim dblGraphTop As Double
        Dim dtemp As Double         ' ###203 

        lMax = 0
        If (m_bFgDispGrp) Then

            'V4.0.0.0�K��
            gGraphAccumulationTitle.Text = MSG_TRIM_04                ' "�C�j�V�����e�X�g�@���z�}"
            gMinValue.Text = dblMinIT.ToString("0.000")               ' �ŏ��l
            gMaxValue.Text = dblMaxIT.ToString("0.000")               ' �ő�l
            'V4.0.0.0�K��
            For iCnt = 0 To (MAX_SCALE_RNUM - 1)
                glRegistNum(iCnt) = glRegistNumIT(iCnt)                 ' ���z�O���t��R��
                If lMax < glRegistNum(iCnt) Then
                    lMax = glRegistNum(iCnt)
                End If
                'V4.0.0.0�K��
                gDistRegNumLblAry(iCnt).Text = CStr(glRegistNum(iCnt)) ' ���z�O���t��R��
                'V4.0.0.0�K��
            Next

            'OK/NG���̕\��
            'V4.0.0.0�K��
            gGoodChip.Text = CStr(gITNx_cnt + 1)                      ' OK��
            gNgChip.Text = CStr(gITNg_cnt + 1)                        ' NG��
            'V4.0.0.0�K��
        Else
            'V4.0.0.0�K��
            gGraphAccumulationTitle.Text = MSG_TRIM_05                ' "�t�@�C�i���e�X�g�@���z�}"
            gMinValue.Text = dblMinFT.ToString("0.000")               ' �ŏ��l
            gMaxValue.Text = dblMaxFT.ToString("0.000")               ' �ő�l
            For iCnt = 0 To (MAX_SCALE_RNUM - 1)

                glRegistNum(iCnt) = glRegistNumFT(iCnt)

                If lMax < glRegistNum(iCnt) Then
                    lMax = glRegistNum(iCnt)
                End If
                gDistRegNumLblAry(iCnt).Text = CStr(glRegistNum(iCnt)) ' ���z�O���t��R��
            Next
            'OK/NG���̕\��
            'V4.0.0.0�K��
            gGoodChip.Text = CStr(gFTNx_cnt + 1)                      ' OK��
            gNgChip.Text = CStr(gFTNg_cnt + 1)                        ' NG��
            'V4.0.0.0�K��
        End If

        'lblGoodChip.Text = CStr(lOkChip)                               ' OK��
        'lblNgChip.Text = CStr(lNgChip)                                 ' NG��


        '������������
        ' �덷�ް�������(IT)
        '' '' ''Call Form1.GetMoveMode(digL, digH, digSW)
        If gITNx_cnt >= 0 Then
            'If (gDigL = 0) Then                                        ' x0���[�h ?
            '' '' ''If (digL = 0) Then                                  ' x0���[�h ?
            '###154 �v�Z�͌��ʎ擾���ɂ��̓s�x���s����
            '' ���ϒl�擾
            'dblAverageIT = Form1.Utility1.GetAverage(gITNx, gITNx_cnt + 1)
            '' �W���΍��̎擾
            'dblDeviationIT = Form1.Utility1.GetDeviation(gITNx, gITNx_cnt + 1, dblAverageIT)
            'TotalDeviationDebug = TotalDeviationDebug '###154
            'TotalAverageDebug = TotalAverageDebug '###154
            '' '' ''End If
        End If

        ' �덷�ް�������(FT)
        If gFTNx_cnt >= 0 Then
            '###154            ' ���ϒl�擾
            '###154            dblAverageFT = Form1.Utility1.GetAverage(gFTNx, gFTNx_cnt + 1)
            '###154     ' �W���΍��̎擾
            '###154         dblDeviationFT = Form1.Utility1.GetDeviation(gFTNx, gFTNx_cnt + 1, dblAverageFT)
            'dblAverageFT = TotalAverageDebug '###154
            'dblDeviationFT = TotalDeviationDebug '###154
        End If
        '��������������

        If (m_bFgDispGrp) Then
            'V4.0.0.0�K��
            gDeviationValue.Text = dblDeviationIT.ToString("0.000000") ' �W���΍�(IT)
            'V4.0.0.0�K��

        Else
            'V4.0.0.0�K��
            gDeviationValue.Text = dblDeviationFT.ToString("0.000000") ' �W���΍�(FT)
            'V4.0.0.0�K��
        End If

        If (m_bFgDispGrp) Then
            dblAverage = dblAverageIT
        Else
            dblAverage = dblAverageFT
        End If
        'V4.0.0.0�K��
        gAverageValue.Text = dblAverage.ToString("0.000")     ' ���ϒl
        'V4.0.0.0�K��
        lScaleMax = 0                                           ' �I�[�g�X�P�[�����O
        lScale = 100
        Do
            If (lScale > lMax) Then                             ' lScale < ��R�� ?
                lScaleMax = lScale
            ElseIf ((lScale * 2) > lMax) Then
                lScaleMax = (lScale * 2)
            ElseIf ((lScale * 5) > lMax) Then
                lScaleMax = (lScale * 5)
            End If
            lScale = lScale * 10
        Loop While (0 = lScaleMax) And (MAX_SCALE_NUM > lScale)

        If (0 = lScaleMax) Then
            lScaleMax = MAX_SCALE_NUM + 1
        End If

        If (m_bFgDispGrp) Then
            If ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            ElseIf ((0 >= typResistorInfoArray(1).dblInitTest_LowLimit) And (0 > typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit / 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            ElseIf ((0 < typResistorInfoArray(1).dblInitTest_LowLimit) And (0 <= typResistorInfoArray(1).dblInitTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblInitTest_HighLimit * 1.5 - typResistorInfoArray(1).dblInitTest_LowLimit / 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblInitTest_HighLimit * 1.5
            Else
                dblGraphDiv = 0.3
                dblGraphTop = 1.5
            End If
        Else
            If ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            ElseIf ((0 >= typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 > typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit / 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit * 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            ElseIf ((0 < typResistorInfoArray(1).dblFinalTest_LowLimit) And (0 <= typResistorInfoArray(1).dblFinalTest_HighLimit)) Then
                dblGraphDiv = (typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5 - typResistorInfoArray(1).dblFinalTest_LowLimit / 1.5) / 10
                dblGraphTop = typResistorInfoArray(1).dblFinalTest_HighLimit * 1.5
            Else
                dblGraphDiv = 0.3
                dblGraphTop = 1.5
            End If
        End If

        'V4.0.0.0�K��
        gDistGrpPerLblAry(0).Text = frmDistribution_005 & dblGraphTop.ToString("0.00")
        'V4.0.0.0�K��
        For iCnt = 1 To 11
            'gDistGrpPerLblAry(iCnt).Text = (dblGraphTop - (dblGraphDiv * (iCnt - 1)).ToString("0.00")) & "�`"
            ' ###203 
            dtemp = (dblGraphTop - (dblGraphDiv * (iCnt - 1)))
            If ((-0.001 < dtemp) And (dtemp < 0.001)) Then
                'V4.0.0.0�K��
                gDistGrpPerLblAry(iCnt).Text = "0" & frmDistribution_005
                'V4.0.0.0�K��
            Else
                'V4.0.0.0�K��
                gDistGrpPerLblAry(iCnt).Text = (dtemp.ToString("0.00")) & frmDistribution_005
                'V4.0.0.0�K��
            End If
            ' ###203
        Next

        picGraphAccumulationDrawLine(lScaleMax)
        Call picGraphAccumulationPrintRegistNum()           ' ���z�O���t�ɒ�R����ݒ肷��

    End Sub
#End Region

#Region "���z�}�\���T�u"
    '''=========================================================================
    '''<summary>���z�}�\���T�u</summary>
    '''<param name="lScaleMax">(INP)�X�P�[��</param>
    '''=========================================================================
    Private Sub picGraphAccumulationDrawLine(ByRef lScaleMax As Integer)

        Dim i As Short
        Dim x As Short

        For i = 0 To (MAX_SCALE_RNUM - 1)
            '            x = CShort((glRegistNum(i) * 473) \ lScaleMax)   ' ���z�O���t��R��
            x = CShort((glRegistNum(i) * 250) \ lScaleMax)   ' ���z�O���t��R��
            'If (473 < x) Then
            If (250 < x) Then
                '                x = 473
                x = 250
            End If
            'V4.0.0.0�K��
            gDistShpGrpLblAry(i).Width = x
            'V4.0.0.0�K��
        Next
        gRegistUnit.Text = CStr(lScaleMax \ 2)            ' ��R���̔����̐� 
    End Sub
#End Region

#Region "���z�O���t�ɒ�R����ݒ肷��"
    '''=========================================================================
    '''<summary>���z�O���t�ɒ�R����ݒ肷��</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub picGraphAccumulationPrintRegistNum()

        Dim i As Short
        'V4.0.0.0�K
        For i = 0 To (MAX_SCALE_RNUM - 1)
            gDistRegNumLblAry(i).Text = CStr(glRegistNum(i))  ' ���z�O���t��R��
        Next
        'V4.0.0.0�K

    End Sub
#End Region

    Private Sub picGraphAccumulation_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picGraphAccumulation.Paint

    End Sub

#Region "�C�j�V�����A�t�@�C�i���t���O�̐ݒ�"
    '''=========================================================================
    ''' <summary>'V4.0.0.0�K
    ''' �C�j�V�����A�t�@�C�i���t���O�̐ݒ�
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetDispGrp(ByVal flg As Boolean)
        m_bFgDispGrp = flg
    End Sub
#End Region

#Region "�C�j�V�����A�t�@�C�i���t���O�̎擾"
    '''=========================================================================
    ''' <summary>'V4.0.0.0�K
    ''' �C�j�V�����A�t�@�C�i���t���O�̎擾
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Function GetDispGrp() As Boolean
        GetDispGrp = m_bFgDispGrp
    End Function

#End Region




    ''' <summary>
    ''' �O���t��O�ʂɐݒ肷�� 'V6.0.2.0�C
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ShowGraph()

        Try

            Me.TopMost = True
            Me.Visible = True
        Catch ex As Exception

        End Try

    End Sub


End Class