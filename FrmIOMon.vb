'===============================================================================
'   Description  : �h�n���j�^
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Public Class FrmIOMon

#Region "�y�ϐ���`�z"
    '===========================================================================
    '   �ϐ���`
    '===========================================================================
    Private LblRBitAry(16) As Label                                     ' ���x��(���̓r�b�g)
    Private LblWBitAry(16) As Label                                     ' ���x��(�o�̓r�b�g)
    Private TimerRD As System.Threading.Timer = Nothing
    Private TimeVal As Integer
    Private mousePoint As Point                                         ' �}�E�X�̃N���b�N�ʒu

#End Region

#Region "�y���\�b�h��`�z"
#Region "Form_Load������"
    '''=========================================================================
    ''' <summary>Form_Load������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ���x���z��ݒ�(���̓r�b�g)
            LblRBitAry(0) = LblReadBit00
            LblRBitAry(1) = LblReadBit01
            LblRBitAry(2) = LblReadBit02
            LblRBitAry(3) = LblReadBit03
            LblRBitAry(4) = LblReadBit04
            LblRBitAry(5) = LblReadBit05
            LblRBitAry(6) = LblReadBit06
            LblRBitAry(7) = LblReadBit07
            LblRBitAry(8) = LblReadBit08
            LblRBitAry(9) = LblReadBit09
            LblRBitAry(10) = LblReadBit0A
            LblRBitAry(11) = LblReadBit0B
            LblRBitAry(12) = LblReadBit0C
            LblRBitAry(13) = LblReadBit0D
            LblRBitAry(14) = LblReadBit0E
            LblRBitAry(15) = LblReadBit0F

            ' ���x���z��ݒ�(�o�̓r�b�g)
            LblWBitAry(0) = LblWriteBit00
            LblWBitAry(1) = LblWriteBit01
            LblWBitAry(2) = LblWriteBit02
            LblWBitAry(3) = LblWriteBit03
            LblWBitAry(4) = LblWriteBit04
            LblWBitAry(5) = LblWriteBit05
            LblWBitAry(6) = LblWriteBit06
            LblWBitAry(7) = LblWriteBit07
            LblWBitAry(8) = LblWriteBit08
            LblWBitAry(9) = LblWriteBit09
            LblWBitAry(10) = LblWriteBit0A
            LblWBitAry(11) = LblWriteBit0B
            LblWBitAry(12) = LblWriteBit0C
            LblWBitAry(13) = LblWriteBit0D
            LblWBitAry(14) = LblWriteBit0E
            LblWBitAry(15) = LblWriteBit0F

            ' ���x���z�񏉊���
            For Idx = 0 To 15
                LblRBitAry(Idx).BackColor = Color.White
                LblWBitAry(Idx).BackColor = Color.White
            Next Idx

            ' �����\���ʒu
            Me.Location = New Point(Form1.Text4.Location.X, 0)

            ' �^�C�}�[�I�u�W�F�N�g�̍쐬(TimerRD_Tick��100msec���TimeVal msec�Ԋu�Ŏ��s����)
            TimeVal = 10                                                ' �^�C�}�[�l(msec)
            TimerRD = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerRD_Tick), Nothing, 100, TimeVal)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)"
    '''=========================================================================
    ''' <summary>�^�C�}�[�C�x���g(�w��^�C�}�Ԋu���o�߂������ɔ���)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerRD_Tick(ByVal Sts As Object)

        Dim strMSG As String

        Try
            ' �R�[���o�b�N���\�b�h�̌ďo�����~����
            TimerRD.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)

            ' �h�n���j�^��\������
            Call IOMonitor()

            ' �^�C�}�X�^�[�g 
            TimerRD.Change(TimeVal, TimeVal)
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmIOMon.TimerRD_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�h�n���j�^��\������"
    '''=========================================================================
    ''' <summary>�h�n���j�^��\������</summary>
    '''=========================================================================
    Private Sub IOMonitor()

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ���[�_����M�f�[�^
            ' 16Bit�f�[�^��ON/OFF���`�F�b�N����
            For Idx = 0 To 15
                If (gLdRDate And (2 ^ Idx)) Then
                    LblRBitAry(Idx).BackColor = Color.Lime
                Else
                    LblRBitAry(Idx).BackColor = Color.White
                End If
            Next Idx

            ' ���[�_�����M�f�[�^
            ' 16Bit�f�[�^��ON/OFF���`�F�b�N����
            For Idx = 0 To 15
                If (gLdWDate And (2 ^ Idx)) Then
                    LblWBitAry(Idx).BackColor = Color.Red
                Else
                    LblWBitAry(Idx).BackColor = Color.White
                End If
            Next Idx
            Exit Sub

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmIOMon.IOMonitor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '=========================================================================
    '   �^�C�g���o�[�������Ȃ��t�H�[���̓}�E�X�ňړ��ł��Ȃ��̂�
    '   ���L��ǉ����ă}�E�X�ňړ��ł���悤�ɂ���
    '=========================================================================
#Region "�}�E�X�̃{�^���������ꂽ�Ƃ�"
    '''=========================================================================
    ''' <summary>�}�E�X�̃{�^���������ꂽ�Ƃ�</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        Dim strMSG As String

        Try
            ' �}�E�X�̍��{�^���������ꂽ�Ƃ��̃}�E�X�̃N���b�N�ʒu���L������
            If (e.Button And System.Windows.Forms.MouseButtons.Left) = System.Windows.Forms.MouseButtons.Left Then
                mousePoint = New Point(e.X, e.Y)                            ' �ʒu���L������
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�}�E�X���������Ƃ�"
    '''=========================================================================
    ''' <summary>�}�E�X���������Ƃ�</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        Dim strMSG As String

        Try
            ' �}�E�X�̍��{�^�����������ăh���b�O�����ʒu�փt�H�[�����ړ�����
            If (e.Button And System.Windows.Forms.MouseButtons.Left) = System.Windows.Forms.MouseButtons.Left Then
                Me.Left += e.X - mousePoint.X
                Me.Top += e.Y - mousePoint.Y
                '�܂��́A���̂悤�ɂ���
                'Me.Location = New Point(Me.Location.X + e.X - mousePoint.X, Me.Location.Y + e.Y - mousePoint.Y)
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_MouseMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region

End Class