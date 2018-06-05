'===============================================================================
'   Description  : ���M���O�R�}���h����
'
'   Copyright(C) : OMRON LASERFRONT INC. 2010
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.Collections.Generic      '#4.12.2.0�E
'
Imports LaserFront.Trimmer.DefWin32Fnc  'V4.4.0.0-0
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Friend Class Logging
    Inherits System.Windows.Forms.Form

    'Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long

#Region "�v���C�x�[�g�ϐ���`"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private mPath As String
    Private strCompPath As String

    Private _trimmingLog As List(Of LogTargetSet)       '#4.12.2.0�E
    Private _measureLog As List(Of LogTargetSet)        '#4.12.2.0�E
#End Region

#Region "�t�H�[�����[�h������"
    '''=========================================================================
    '''<summary>�t�H�[�����[�h������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Logging_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim frmSize As System.Drawing.Size

        Me.Text = TITLE_LOGGING

        '�t�H�[���\���T�C�Y�̕ύX
        frmSize = Me.Size
        'frmSize.Height = 220
        frmSize.Height = 230
        chkOptDisp.Text = "Option >>"
        Me.Size = frmSize

        ' �R���g���[���̏�����
        Check2.CheckState = gSysPrm.stLOG.giLoggingMode     ' �����ޯ��(۷�ݸ�Ӱ��) 
        Check1.CheckState = gSysPrm.stLOG.giLoggingAppend   ' �����ޯ��(̧�ٱ�����Ӱ��) �����g�p 
        Text1.Text = gSysPrm.stLOG.gsLoggingDir & gSysPrm.stLOG.gsLoggingFile
        strCompPath = Text1.Text
        UDLotNum.Value = gSysPrm.stLOG.giLoggingLotNo       ' ۸� ۯĔԍ�

        ' Initial Test/Final Test�`�F�b�N�{�b�N�X(���g�p ?)
        If (1 = gSysPrm.stLOG.giLoggingDataKind) Or (3 = gSysPrm.stLOG.giLoggingDataKind) Then
            chkLogInitial.CheckState = System.Windows.Forms.CheckState.Checked
        Else
            chkLogInitial.CheckState = System.Windows.Forms.CheckState.Unchecked
        End If
        If (2 = gSysPrm.stLOG.giLoggingDataKind) Or (3 = gSysPrm.stLOG.giLoggingDataKind) Then
            chkLogFinal.CheckState = System.Windows.Forms.CheckState.Checked
        Else
            chkLogFinal.CheckState = System.Windows.Forms.CheckState.Unchecked
        End If

        ' ۷�ݸ�����(�����ޯ��)
        Combo2.Items.Add("1: Initial")
        Combo2.Items.Add("2: Final")
        Combo2.Items.Add("3: Initial & Final")
        Combo2.SelectedIndex = gSysPrm.stLOG.giLoggingDataKind - 1

        ' ۷�ݸ�Ӱ��=�Ȃ��Ȃ�ȉ��͔񊈐����Ƃ���
        If gSysPrm.stLOG.giLoggingMode = 0 Then
            Text1.Enabled = False
            Command3.Enabled = False
            Check1.Enabled = False
            UDLotNum.Enabled = False
            Combo2.Enabled = False
        End If

        ' ���茋�ʒǉ��@�\(�W�����M���O�f�[�^�Ɍ덷���Ɣ��茋�ʁg�k�h���́g�g�h��ǉ�)
        '' '' ''chkResultAdd.Visible = True
        chkResultAdd.CheckState = gSysPrm.stLOG.giLogResult

        '�I�v�V�����@�\�̃h���b�v�_�E�����X�g��ݒ�
        '�I�v�V�����{�^���̕\��/��\�����������{
        '' '' ''chkOptDisp.Visible = True
#If True Then                           '#4.12.2.0�E �I�v�V�����@�\�A�g�p���Ă��Ȃ�
        InitializeDataGridView(DataGridViewTrimmingLog, _trimmingLog)
        InitializeDataGridView(DataGridViewMeasureLog, _measureLog)
#Else
        cmbTrimTarNo01.SelectedIndex = 0
        cmbTrimTarNo02.SelectedIndex = 1
        cmbTrimTarNo03.SelectedIndex = 2
        cmbTrimTarNo04.SelectedIndex = 3
        cmbTrimTarNo05.SelectedIndex = 4
        cmbTrimTarNo06.SelectedIndex = 5
        cmbTrimTarNo07.SelectedIndex = 6
        cmbTrimTarNo08.SelectedIndex = 7
        cmbTrimTarNo09.SelectedIndex = 8
        cmbTrimTarNo10.SelectedIndex = 9
        cmbTrimTarNo11.SelectedIndex = 10
        cmbTrimTarNo12.SelectedIndex = 11
        cmbMeasTarNo01.SelectedIndex = 0
        cmbMeasTarNo02.SelectedIndex = 1
        cmbMeasTarNo03.SelectedIndex = 2
        cmbMeasTarNo04.SelectedIndex = 3
        cmbMeasTarNo05.SelectedIndex = 4
        cmbMeasTarNo06.SelectedIndex = 5
        cmbMeasTarNo07.SelectedIndex = 6
        cmbMeasTarNo08.SelectedIndex = 7
        cmbMeasTarNo09.SelectedIndex = 8
        cmbMeasTarNo10.SelectedIndex = 9
        cmbMeasTarNo11.SelectedIndex = 10
        cmbMeasTarNo12.SelectedIndex = 11
#End If
    End Sub
#End Region

    '#Region "CloseMag���݉���������"
    '    '''=========================================================================
    '    '''<summary>CloseMag���݉���������</summary>
    '    '''<remarks>���g�p ?</remarks>
    '    '''=========================================================================
    '    Private Sub CmdLogTyp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CmdLogTyp.Click

    '        If gSysPrm.stDEV.rLogTyp_flg = True Then
    '            ' ON->OFF
    '            gSysPrm.stDEV.rLogTyp_flg = False
    '            CmdLogTyp.Text = "CloseMag OFF"
    '            CmdLogTyp.BackColor = System.Drawing.ColorTranslator.FromOle(&H8000000F)
    '        Else
    '            ' OFF->ON
    '            gSysPrm.stDEV.rLogTyp_flg = True
    '            CmdLogTyp.Text = "CloseMag ON"
    '            CmdLogTyp.BackColor = System.Drawing.ColorTranslator.FromOle(&HFF00)
    '        End If

    '    End Sub
    '#End Region

#Region "Cancel���݉���������"
    '''=========================================================================
    '''<summary>Cancel���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Command1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command1.Click

        Me.Close()

    End Sub
#End Region

#Region "OK���݉���������"
    '''=========================================================================
    '''<summary>OK���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Command2_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command2.Click
        'V4.0.0.0-65                ��������
        Dim prevLoggingMode As Integer = gSysPrm.stLOG.giLoggingMode
        ' Logging��ON����OFF�ɐݒ肳�ꂽ�ꍇ
        If (1 = prevLoggingMode) AndAlso (False = Check2.Checked) AndAlso (True = Globals_Renamed.gLoadDTFlag) Then
            basTrimming.TrimLogging_CreateOrAppend("Logging - OFF", procMsgOnly:=True)
        End If
        'V4.0.0.0-65                ��������
        gSysPrm.stLOG.giLoggingMode = Check2.CheckState                         ' �����ޯ��(۷�ݸ�Ӱ��) 
        gSysPrm.stLOG.giLoggingAppend = Check1.CheckState                       ' �����ޯ��(̧�ٱ�����Ӱ��) �����g�p 
        gSysPrm.stLOG.giLoggingLotNo = UDLotNum.Value                           ' ۸� ۯĔԍ�
        gSysPrm.stLOG.giLoggingDataKind = CShort(Combo2.Text.Substring(0, 1))   ' ۷�ݸ�����

        ' ۷�ݸ�̧�قɌ덷�Ɣ��茋�ʂ�ǉ�
        gSysPrm.stLOG.giLogResult = chkResultAdd.CheckState

        '----- ###224�� -----
        ' �V�X�e���p�����[�^�[�̕ۑ�([LOGGING]�Z�N�V�����̂ݍX�V����)
        'Call gDllSysprmSysParam_definst.PutSystemParameter(gSysPrm)
        Call gDllSysprmSysParam_definst.PutSysPrm_LOGGING(gSysPrm.stLOG)
        '----- ###224�� -----

        'V4.0.0.0-65                ��������
        ' ۸ޕ\���J�n�׸ނ���������B�ް�̧�ٓǂݍ��ݍς݂̏ꍇ
        If (1 = gSysPrm.stLOG.giLoggingMode) AndAlso (True = Globals_Renamed.gLoadDTFlag) Then
            ' ۸�̧�ٖ����ύX���ꂽ�ꍇ�܂���Logging��OFF����ON�ɐݒ肳�ꂽ�ꍇ
            If (strCompPath <> Text1.Text) OrElse (0 = prevLoggingMode) Then
                basTrimming.TrimLogging_CreateOrAppend("Logging - ON", typPlateInfo.strDataName)
            End If
        End If
        'V4.0.0.0-65                ��������

        Me.Close()

    End Sub
#End Region

#Region "File name���݉���������"
    '''=========================================================================
    '''<summary>File name���݉���������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub Command3_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Command3.Click

        Dim n As Short

        If gSysPrm.stLOG.gsLoggingDir <> "" Then
            comDlgSave.InitialDirectory = gSysPrm.stLOG.gsLoggingDir
        End If
        If gSysPrm.stLOG.gsLoggingFile <> "" Then
            comDlgSave.FileName = gSysPrm.stLOG.gsLoggingFile
        End If

        comDlgSave.Title = "Log file name"
        comDlgSave.DefaultExt = "*.txt"
        comDlgSave.FilterIndex = 2

        comDlgSave.Filter = "*.log|*.log|*.txt|*.txt|*.*|*.*"
        comDlgSave.ShowDialog()
        If comDlgSave.FileName = "" Then Exit Sub
        mPath = comDlgSave.FileName

        If mPath = "" Then Exit Sub

        n = InStrRev(mPath, "\")
        If n Then
            gSysPrm.stLOG.gsLoggingDir = mPath.Substring(0, n)
            gSysPrm.stLOG.gsLoggingFile = Mid(mPath, n + 1)
            Text1.Text = gSysPrm.stLOG.gsLoggingDir & gSysPrm.stLOG.gsLoggingFile
        End If

    End Sub
#End Region

#Region "�����ޯ��(۷�ݸ�Ӱ��)��ݼގ�����"
    '''=========================================================================
    '''<summary>�����ޯ��(۷�ݸ�Ӱ��)��ݼގ�����</summary>
    '''<remarks>CheckStateChanged �́A�t�H�[�������������ꂽ�Ƃ��ɔ���</remarks>
    '''=========================================================================
    Private Sub Check2_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Check2.CheckStateChanged

        If Check2.CheckState Then
            Text1.Enabled = True
            Command3.Enabled = True
            Check1.Enabled = True
            UDLotNum.Enabled = True
            Combo2.Enabled = True
        Else
            Text1.Enabled = False
            Command3.Enabled = False
            Check1.Enabled = False
            UDLotNum.Enabled = False
            Combo2.Enabled = False
        End If

    End Sub
#End Region

    Private Sub chkOptDisp_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOptDisp.CheckedChanged
        Dim frmSize As System.Drawing.Size

        Try
            frmSize = Me.Size

            If chkOptDisp.Checked = True Then
                frmSize.Height = 630
                chkOptDisp.Text = "Option <<"
            Else
                frmSize.Height = 220
                chkOptDisp.Text = "Option >>"
            End If
            Me.Size = frmSize

        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try
    End Sub

    Private Sub btnSetLogTarget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetLogTarget.Click
        Dim path As String = "C:\TRIM\LogTargetFile.ini"

        Try
#If True Then                           '#4.12.2.0�E �I�v�V�����@�\�A�g�p���Ă��Ȃ�
            'TrimmingLog�̃L�[��ݒ�
            For Each tl As LogTargetSet In _trimmingLog
                WritePrivateProfileString(cTRIMLOGcSECTNAME, tl.Order, tl.LogTarget.ToString(), path)
            Next

            'MeasLog�̃L�[��ݒ�
            For Each ml As LogTargetSet In _measureLog
                WritePrivateProfileString(cMEASLOGcSECTNAME, ml.Order, ml.LogTarget.ToString(), path)
            Next
#Else
            'TrimmingLog�̃L�[��ݒ�
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "1", cmbTrimTarNo01.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "2", cmbTrimTarNo02.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "3", cmbTrimTarNo03.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "4", cmbTrimTarNo04.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "5", cmbTrimTarNo05.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "6", cmbTrimTarNo06.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "7", cmbTrimTarNo07.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "8", cmbTrimTarNo08.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "9", cmbTrimTarNo09.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "10", cmbTrimTarNo10.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "11", cmbTrimTarNo11.Text.ToString(), path)
            Call WritePrivateProfileString(cTRIMLOGcSECTNAME, "12", cmbTrimTarNo11.Text.ToString(), path)
            'MeasLog�̃L�[��ݒ�
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "1", cmbMeasTarNo01.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "2", cmbMeasTarNo02.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "3", cmbMeasTarNo03.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "4", cmbMeasTarNo04.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "5", cmbMeasTarNo05.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "6", cmbMeasTarNo06.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "7", cmbMeasTarNo07.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "8", cmbMeasTarNo08.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "9", cmbMeasTarNo09.Text.ToString(), path)
            Call WritePrivateProfileString(cMEASLOGcSECTNAME, "10", cmbMeasTarNo10.Text.ToString(), path)
#End If
        Catch ex As Exception
            MsgBox("Execption error !" & vbCrLf & "error msg = " & ex.Message)
        End Try

    End Sub

#Region "'#4.12.2.0�E �I�v�V�����@�\�A�g�p���Ă��Ȃ�"
    ''' <summary></summary>
    ''' <param name="dataGridVeiw"></param>
    ''' <param name="kind"></param>
    Private Sub InitializeDataGridView(ByVal dataGridVeiw As DataGridView, ByRef kind As List(Of LogTargetSet))
        kind = New List(Of LogTargetSet)()
        For Each value As gwModule.LOGTAR In [Enum].GetValues(GetType(gwModule.LOGTAR))
            kind.Add(New LogTargetSet(value))
        Next

        Dim order As New DataGridViewTextBoxColumn With
        {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
            .DataPropertyName = "Order",
            .HeaderText = "Order",
            .ReadOnly = True,
            .SortMode = DataGridViewColumnSortMode.NotSortable
        }
        order.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        order.DefaultCellStyle.BackColor = SystemColors.Control
        order.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim logTarget As New DataGridViewComboBoxColumn With
        {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .DataSource = LogTargetSet.Target,
            .DataPropertyName = "LogTarget",
            .DisplayMember = "Key",
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox,
            .HeaderText = "LogTarget",
            .SortMode = DataGridViewColumnSortMode.NotSortable,
            .ValueMember = "Value"
        }
        logTarget.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        dataGridVeiw.AutoGenerateColumns = False
        dataGridVeiw.DataSource = kind
        dataGridVeiw.Columns.AddRange(order, logTarget)

    End Sub

    Private Class LogTargetSet
        Public Shared ReadOnly Target As List(Of KeyValuePair(Of String, gwModule.LOGTAR))

        Private _order As Integer
        Public ReadOnly Property Order() As Integer
            Get
                Return _order
            End Get
        End Property

        Private _logTarget As gwModule.LOGTAR
        Public Property LogTarget() As gwModule.LOGTAR
            Get
                Return _logTarget
            End Get
            Set(ByVal value As gwModule.LOGTAR)
                _logTarget = value
            End Set
        End Property

        Shared Sub New()
            Target = New List(Of KeyValuePair(Of String, LOGTAR))()
            Dim type As Type = GetType(gwModule.LOGTAR)
            Dim keys() As String = [Enum].GetNames(type)
            Dim i As Integer = 0
            For Each value As [Enum] In [Enum].GetValues(type)
                Target.Add(New KeyValuePair(Of String, LOGTAR)(keys(i), value))
                i += 1
            Next
        End Sub

        Public Sub New(ByVal target As gwModule.LOGTAR)
            _order = CInt(target) + 1
            _logTarget = target
        End Sub
    End Class

    Private Sub DataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles _
        DataGridViewTrimmingLog.CellClick, DataGridViewMeasureLog.CellClick

        If (0 <= e.ColumnIndex) AndAlso (0 <= e.RowIndex) Then
            Dim dgv As DataGridView = DirectCast(sender, DataGridView)
            If (TypeOf dgv.Columns(e.ColumnIndex) Is DataGridViewComboBoxColumn) Then
                dgv.BeginEdit(True)
                DirectCast(dgv.EditingControl, ComboBox).DroppedDown = True
            End If
        End If
    End Sub
#End Region

End Class
