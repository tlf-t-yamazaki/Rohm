Option Explicit On
Option Strict On

Imports System.Threading
Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.DllSystem

''' <summary>���b�g�I�����̎����^�]�I���Ċm�F���b�Z�[�W</summary>
''' <remarks>'V6.0.3.0_21</remarks>
Public Class FormLotEnd

    Private Shared _input As Integer
    ''' <summary>���b�g���������</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Input() As Integer
        Get
            Return _input
        End Get
        Set(ByVal value As Integer)
            _input = value
        End Set
    End Property

    Private Shared _processed As Integer
    ''' <summary>���������</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Processed() As Integer
        Get
            Return _processed
        End Get
        Set(ByVal value As Integer)
            _processed = value
        End Set
    End Property

    Private Shared _allowable As Integer
    ''' <summary>���e�����</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property Allowable() As Integer
        Get
            Return _allowable
        End Get
        Set(ByVal value As Integer)
            _allowable = value
        End Set
    End Property

    ''' <summary>�����^�]�I���̍Ċm�F�������Ȃ����ǂ���</summary>
    ''' <returns>true:�����Ȃ�,false:�����Ȃ�Ȃ�</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property DoConfirm() As Boolean
        Get
            Return (_allowable < (_input - _processed))
        End Get
    End Property

    Private _exitFlag As Integer
    ''' <summary>�߂�l</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExitFlag() As Integer
        Get
            Return _exitFlag
        End Get
        Private Set(ByVal value As Integer)
            SyncLock (_writeLock)
                If (cFRS_NORMAL = _exitFlag) Then
                    _exitFlag = value
                End If
            End SyncLock
        End Set
    End Property

    Private _writeLock As Object
    Private _msgTyp As Integer

#Region "�R���X�g���N�^"
    ''' <summary>�R���X�g���N�^</summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal msgTyp As Integer)

        ' ���̌Ăяo���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()

        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B
        _writeLock = New Object()
        _exitFlag = cFRS_NORMAL
        _msgTyp = msgTyp

    End Sub
#End Region

#Region "���͑҂�"
    ''' <summary>���͑҂�</summary>
    ''' <param name="ObjSys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Execute(ByVal ObjSys As SystemNET) As Integer
        Dim r As Integer
        Dim sts As Integer

        Try
            Do
                ' START/RESET�L�[�����҂�
                r = STARTRESET_SWCHECK(0, sts)      ' START/RESET SW�����`�F�b�N
                If (r = cFRS_ERR_EMG) Then
                    ExitFlag = cFRS_ERR_EMG
                    Exit Do
                End If

                ' RESET�L�[���� ?
                If (sts = cFRS_ERR_RST) Then        ' RESET�L�[����Cancel�{�^������ ? 
                    ExitFlag = cFRS_ERR_RST         ' Retuen�l = RESET�L�[����Cancel�{�^������
                    Exit Do
                End If

                ' START�L�[���� ?
                If (sts = cFRS_ERR_START) Then      ' START�L�[����OK�{�^������ ?
                    ExitFlag = cFRS_ERR_START       ' Retuen�l = START�L�[����OK�{�^������
                    Exit Do
                End If

                Application.DoEvents()              ' ���b�Z�[�W�|���v
                Thread.Sleep(100)                   ' Wait(msec)
                If ObjSys.EmergencySwCheck() Then   ' ����~ ?
                    ExitFlag = cFRS_ERR_EMG         ' Retuen�l = ����~
                    Exit Do
                End If

            Loop While (cFRS_NORMAL = _exitFlag)

        Catch ex As Exception
            ExitFlag = cERR_TRAP                    ' Retuen�l = �ׯ�ߴװ����
        End Try

        Return _exitFlag

    End Function
#End Region

#Region "�C�x���g"
    Private Sub FormLotEnd_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If (0 <> _msgTyp) Then
            Me.LabelMessage.Text = "There is not enough processing." &
                Environment.NewLine & "Do you want to end automatic operation?"
            Me.LabelLotInput.Text = "Number of Lot Input"
            Me.LabelProcessed.Text = "Number of Processed"
            Me.LabelAllowable.Text = "Number of Allowable"
            Me.ButtonContinue.Text = "Cont."
            Me.ButtonEnd.Text = "End"
        End If

        Me.LabelInNum.Text = FormLotEnd.Input.ToString("0")
        Me.LabelProcNum.Text = FormLotEnd.Processed.ToString("0")
        Me.LabelAlbNum.Text = FormLotEnd.Allowable.ToString("0")
    End Sub

    Private Sub ButtonContinue_Click(sender As Object, e As EventArgs) Handles ButtonContinue.Click
        ExitFlag = cFRS_ERR_START
    End Sub

    Private Sub ButtonEnd_Click(sender As Object, e As EventArgs) Handles ButtonEnd.Click
        ExitFlag = cFRS_ERR_RST
    End Sub
#End Region

End Class