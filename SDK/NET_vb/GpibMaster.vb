'==============================================================================
'   Description : �b�n�m�s�d�b�@�f�o�h�a�ʐM�p�N���X
'
'   Copyright(C): OMRON Laser Front 2012
'
'==============================================================================
Option Strict Off
Option Explicit On 

Public Class GpibMaster
    '==========================================================================
    '   �萔/�ϐ���`
    '==========================================================================
#Region "�b�n�m�s�d�b�@�f�o�h�a�ʐM�p�c�k�k��`"
    '--------------------------------------------------------------------------
    '   �b�n�m�s�d�b�@�f�o�h�a�ʐM�p�c�k�k��`
    '--------------------------------------------------------------------------
    ' Common
    Declare Function GpibInit Lib "CGPIB.DLL" (ByVal strDeviceName As String, ByRef shDevId As Short) As Integer
    Declare Function GpibExit Lib "CGPIB.DLL" (ByVal shDevId As Short) As Integer
    Declare Function GpibGetErrorString Lib "CGPIB.DLL" (ByVal intErrorCode As Integer, ByVal strErrorString As System.Text.StringBuilder) As Integer

    ' Initialization
    Declare Function GpibSetEquipment Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shEqpId As Short, ByVal shPrmAddr As Short, ByVal shScdAddr As Short, ByVal shDelim As Short) As Integer
    Declare Function GpibGetEquipment Lib "CGPIB.DLL" (ByVal shEqpId As Short, ByRef shPrmAddr As Short, ByRef shScdAddr As Short, ByRef shDelim As Short) As Integer
    Declare Function GpibSendIFC Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shIfcTime As Short) As Integer
    Declare Function GpibChangeREN Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shEnable As Short) As Integer
    Declare Function GpibEnableRemote Lib "CGPIB.DLL" (ByVal shId As Short) As Integer

    ' Send and Reception
    Declare Function GpibSendData Lib "CGPIB.DLL" (ByVal shId As Short, ByRef intSendLen As Integer, ByVal strSendBuf As System.Text.StringBuilder) As Integer
    Declare Function GpibRecData Lib "CGPIB.DLL" (ByVal shId As Short, ByRef intRecLen As Integer, ByVal strRecBuf As System.Text.StringBuilder) As Integer
    Declare Function GpibSetDelim Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shDelim As Short, ByVal shEoi As Short, ByVal shEos As Short) As Integer
    Declare Function GpibGetDelim Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shDelim As Short, ByRef shEoi As Short, ByRef shEos As Short) As Integer
    Declare Function GpibSetTimeOut Lib "CGPIB.DLL" (ByVal shId As Short, ByVal intTimeOut As Integer) As Integer
    Declare Function GpibGetTimeOut Lib "CGPIB.DLL" (ByVal shId As Short, ByRef intTimeOut As Integer) As Integer
    Declare Function GpibSetEscape Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shEnable As Short, ByVal shKeyType As Short, ByVal intKeyCode As Integer) As Integer
    Declare Function GpibGetEscape Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shEnable As Short, ByVal shKeyType As Short, ByRef intKeyCode As Integer) As Integer
    Declare Function GpibSetSlowMode Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shSlowTime As Short) As Integer
    Declare Function GpibGetSlowMode Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shSlowTime As Short) As Integer
    Declare Function GpibSetSmoothMode Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shMode As Short) As Integer
    Declare Function GpibGetSmoothMode Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shMode As Short) As Integer
    Declare Function GpibSetAddrInfo Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shTalker As Short, ByRef shListenerArray As Short) As Integer
    Declare Function GpibGetAddrInfo Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shTalker As Short, ByRef shListenerArray As Short) As Integer

    ' Serial poll
    Declare Function GpibSPoll Lib "CGPIB.DLL" (ByVal shEqpId As Short, ByRef shStb As Short, ByRef shSrq As Short) As Integer
    Declare Function GpibSPollAll Lib "CGPIB.DLL" (ByVal shId As Short, ByRef shAddrArray As Short, ByRef shStbArray As Short, ByRef shSrqArray As Short) As Integer

    ' Parallel poll
    Declare Function GpibPPoll Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shPpr As Short) As Integer
    Declare Function GpibSetPPoll Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shAddrArray As Short, ByRef shDataLineArray As Short, ByRef shPolarityArray As Short) As Integer
    Declare Function GpibGetPPoll Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shAddrArray As Short, ByRef shDataLineArray As Short, ByRef shPolarityArray As Short) As Integer
    Declare Function GpibResetPPoll Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shAddrArray As Short) As Integer

    ' SRQ response
    Declare Function GpibSetPPollResponse Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal shResponse As Short) As Integer
    Declare Function GpibGetPPollResponse Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shResponse As Short) As Integer
    Declare Function GpibSetIst Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal shIst As Short) As Integer
    Declare Function GpibGetIst Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shIst As Short) As Integer
    Declare Function GpibSendSRQ Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal shSrqSend As Short, ByVal shStb As Short) As Integer
    Declare Function GpibCheckSPoll Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shSPoll As Short, ByRef shStb As Short) As Integer

    ' Control
    Declare Function GpibSendCommands Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef shCmdArray As Short) As Integer
    Declare Function GpibSendTrigger Lib "CGPIB.DLL" (ByVal shId As Short) As Integer
    Declare Function GpibSendDeviceClear Lib "CGPIB.DLL" (ByVal shId As Short) As Integer
    Declare Function GpibChangeLocal Lib "CGPIB.DLL" (ByVal shId As Short) As Integer
    Declare Function GpibSendLocalLockout Lib "CGPIB.DLL" (ByVal shDevId As Short) As Integer

    ' Status
    Declare Function GpibSetStatus Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shSelect As Short, ByVal intData As Integer) As Integer
    Declare Function GpibGetStatus Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shSelect As Short, ByRef intData As Integer) As Integer
    Declare Function GpibReadLines Lib "CGPIB.DLL" (ByVal shId As Short, ByVal shSelect As Short, ByRef shLineStatus As Short) As Integer
    Declare Function GpibFindListener Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal shPrmAddr As Short, ByVal shScdAddr As Short, ByRef shArraySize As Short, ByRef shAddrArray As Short) As Integer
    Declare Function GpibGetSignal Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef intwParam As Integer, ByRef intlParam As Integer) As Integer

    'Event
    Declare Function GpibSetNotifySignal Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal hWnd As Integer, ByVal intNotifySignalMask As Integer) As Integer
    Declare Function GpibGetNotifySignal Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef hWnd As Integer, ByRef intNotifySignalMask As Integer) As Integer
    Declare Function GpibSetNotifyMessage Lib "CGPIB.DLL" (ByVal shDevId As Short, ByVal intMessage As Integer) As Integer
    Declare Function GpibGetNotifyMessage Lib "CGPIB.DLL" (ByVal shDevId As Short, ByRef intMessage As Integer) As Integer
#End Region

    '=========================================================================
    '   ���\�b�h��`
    '=========================================================================
#Region "����������"
    '''=========================================================================
    ''' <summary>����������</summary>
    ''' <param name="strDeviceName">(INP)�f�o�C�X��(��)"GPIB000")</param>
    ''' <param name="mintDevId">    (OUT)�f�o�C�XID</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ''' <remarks>�f�o�C�X������f�o�C�XID���擾����</remarks>
    '''=========================================================================
    Public Function Gpib_Init(ByVal strDeviceName As String, ByRef mintDevId As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim lngData As Integer
        Dim mlngRet As Integer
        Dim strMSG As String

        Try
            ' �������֐����s
            mlngRet = GpibInit(strDeviceName, mintDevId)                ' ������
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)                ' ���b�Z�[�W�擾 
                MsgBox(strTmp.ToString)

            Else ' ����Ƀf�o�C�XID���擾�ł����ꍇ
                strTmp = New System.Text.StringBuilder("GpibInit : �f�o�C�XID = " & mintDevId)
                ' �@��A�h���X�̎擾
                mlngRet = GpibGetStatus(mintDevId, &H8S, lngData)
                ' �v���p�e�B�Ń}�X�^�ɐݒ肳��Ă��邩�m�F
                mlngRet = GpibGetStatus(mintDevId, &HAS, lngData)
                ' �}�X�^�ɐݒ肳��Ă��邩�m�F
                If (mlngRet = 0) And (lngData = 1) Then
                    Call MsgBox("�}�X�^�ɐݒ肳��Ă��܂���B�v���p�e�B�Ń}�X�^�ɐݒ肵�Ă��������B", MsgBoxStyle.OkOnly)
                End If
            End If

            ' IFC�𑗏o
            GpibSendIFC(mintDevId, 1)
            Return (mlngRet)                                            ' �f�o�C�XID��Ԃ�

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.Gpib_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�I������"
    '''=========================================================================
    ''' <summary>�I������</summary>
    ''' <param name="mintDevId">(INP)�f�o�C�XID</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ''' <remarks>�f�o�C�XID�𖳌��ɂ���</remarks>
    '''=========================================================================
    Public Function Gpib_Term(ByVal mintDevId As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim mlngRet As Integer
        Dim strMSG As String

        Try
            ' �I���֐����s
            If (mintDevId) Then Return (0) '                            ' �f�o�C�XID�Ȃ��Ȃ�NOP
            mlngRet = GpibExit(mintDevId)                               ' �I��
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
                MsgBox(strTmp.ToString)
            End If
            Return (mlngRet)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.Gpib_Term() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�f�[�^���M����"
    '''=========================================================================
    ''' <summary>�f�[�^���M����</summary>
    ''' <param name="strSendDat">(INP)���M�f�[�^</param>
    ''' <param name="mintDevId"> (INP)�f�o�C�XID</param>
    ''' <param name="Addr">      (INP)GPIB�A�h���X</param>
    ''' <param name="Delim">     (INP)�f���~�^�ݒ�(0:�g�p���Ȃ�, 1:CR+LF, 2:CR, 3:LF)</param>
    ''' <param name="Eoi">       (INP)EOI(0:�o�͂��Ȃ�, 0�ȊO:�o�͂���)</param>
    ''' <remarks>�X���[�u�@��Ƀf�[�^�𑗐M����</remarks>
    '''=========================================================================
    Public Function Gpib_Send(ByVal strSendDat As String, ByVal mintDevId As Short, ByVal Addr As Short, ByVal Delim As Short, ByVal Eoi As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim strSendBuf As System.Text.StringBuilder
        Dim lngSendLen As Integer
        Dim mlngRet As Integer
        Dim EqpId As Short
        Dim strMSG As String

        Try
            ' ����@��̐ݒ�
            mlngRet = funcSetParam(mintDevId, Addr, Delim, Eoi, EqpId)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                Return (mlngRet)
            End If

            ' ���M�f�[�^�`�F�b�N
            strSendBuf = New System.Text.StringBuilder(strSendDat)
            lngSendLen = strSendBuf.Length
            If (lngSendLen = 0) Then
                strTmp = New System.Text.StringBuilder("Gpib_Send : ���M�f�[�^������܂���")
                MsgBox(strTmp.ToString)
                Return (1)
            End If

            ' ���M���s
            mlngRet = GpibSendData(EqpId, lngSendLen, strSendBuf)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
                MsgBox(strTmp.ToString)
            End If
            Return (mlngRet)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.Gpib_Send() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�f�[�^��M����"
    '''=========================================================================
    ''' <summary>�f�[�^��M����</summary>
    ''' <param name="strRecDat">(OUT)��M�f�[�^</param>
    ''' <param name="mintDevId">(INP)�f�o�C�XID</param>
    ''' <param name="Addr">     (INP)GPIB�A�h���X</param>
    ''' <param name="Delim">    (INP)�f���~�^�ݒ�(0:�g�p���Ȃ�, 1:CR+LF, 2:CR, 3:LF)</param>
    ''' <param name="Eoi">      (INP)EOI(0:�o�͂��Ȃ�, 0�ȊO:�o�͂���)</param>
    ''' <remarks>�X���[�u�@�킩��f�[�^����M����</remarks>
    '''=========================================================================
    Public Function Gpib_Recv(ByRef strRecDat As String, ByVal mintDevId As Short, ByVal Addr As Short, ByVal Delim As Short, ByVal Eoi As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim lngRecLen As Integer
        Dim strRecBuf As System.Text.StringBuilder
        Dim mlngRet As Integer
        Dim EqpId As Short
        Dim strDAT As String
        Dim strMSG As String

        Try
            mlngRet = funcSetParam(mintDevId, Addr, Delim, Eoi, EqpId)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                Return (mlngRet)
            End If

            ' ��M���s
            lngRecLen = 256
            strRecBuf = New System.Text.StringBuilder("", 256)
            mlngRet = GpibRecData(EqpId, lngRecLen, strRecBuf)
            strRecBuf.Length = lngRecLen

            ' �߂�l����
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
            Else
                '�\��
                strRecBuf.Length = lngRecLen
                'strRecDat = VB.Left(strRecBuf.ToString, lngRecLen) '��M�f�[�^�ݒ�
                strDAT = strRecBuf.ToString                             ' ��M�f�[�^�擾
                strRecDat = strDAT.Substring(0, lngRecLen)              ' ��M�f�[�^��Ԃ�
                strTmp = New System.Text.StringBuilder("�u�f�[�^�̎�M�v����I��")
            End If
            Return (mlngRet)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.Gpib_Recv() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "�f�[�^��M����(Double�^�f�[�^�ŕԂ�)"
    '''=========================================================================
    ''' <summary>�f�[�^��M����</summary>
    ''' <param name="dblRecDat">(OUT)��M�f�[�^</param>
    ''' <param name="mintDevId">(INP)�f�o�C�XID</param>
    ''' <param name="Addr">     (INP)GPIB�A�h���X</param>
    ''' <param name="Delim">    (INP)�f���~�^�ݒ�(0:�g�p���Ȃ�, 1:CR+LF, 2:CR, 3:LF)</param>
    ''' <param name="Eoi">      (INP)EOI(0:�o�͂��Ȃ�, 0�ȊO:�o�͂���)</param>
    ''' <remarks>�X���[�u�@�킩��f�[�^����M����</remarks>
    '''=========================================================================
    Public Function Gpib_RVal(ByRef dblRecDat As Double, ByVal mintDevId As Short, ByVal Addr As Short, ByVal Delim As Short, ByVal Eoi As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim lngRecLen As Integer
        Dim strRecBuf As System.Text.StringBuilder
        Dim mlngRet As Integer
        Dim EqpId As Short
        Dim strDAT As String
        Dim strMSG As String

        Try
            mlngRet = funcSetParam(mintDevId, Addr, Delim, Eoi, EqpId)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                Return (mlngRet)
            End If

            ' ��M���s
            lngRecLen = 256
            strRecBuf = New System.Text.StringBuilder("", 256)
            mlngRet = GpibRecData(EqpId, lngRecLen, strRecBuf)
            strRecBuf.Length = lngRecLen

            ' �߂�l����
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
            Else
                '�\��
                strRecBuf.Length = lngRecLen
                strDAT = strRecBuf.ToString                             ' ��M�f�[�^�擾
                strMSG = strDAT.Substring(0, lngRecLen)                 ' 
                dblRecDat = Double.Parse(strMSG)                        ' ��M�f�[�^��Double�^�f�[�^�ŕԂ� 
                strTmp = New System.Text.StringBuilder("�u�f�[�^�̎�M�v����I��")
            End If
            Return (mlngRet)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.Gpib_RVal() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

#Region "����@��̐ݒ�"
    '''=========================================================================
    ''' <summary>����@��̐ݒ�</summary>
    ''' <param name="mintDevId">(INP)�f�o�C�XID</param>
    ''' <param name="Addr">     (INP)GPIB�A�h���X</param>
    ''' <param name="Delim">    (INP)�f���~�^�ݒ�(0:�g�p���Ȃ�, 1:CR+LF, 2:CR, 3:LF)</param>
    ''' <param name="Eoi">      (INP)EOI(0:�o�͂��Ȃ�, 0�ȊO:�o�͂���)</param>
    ''' <param name="EqpId">    (OUT)�C�N�E�B�b�v�����gID</param>
    ''' <returns>0 = ����, 0�ȊO = �G���[</returns>
    ''' <remarks>�C�N�E�B�b�v�����gID�E�f���~�^�EEOI�̐ݒ���s���܂�</remarks>
    '''=========================================================================
    Private Function funcSetParam(ByVal mintDevId As Short, ByVal Addr As Short, ByVal Delim As Short, ByVal Eoi As Short, ByRef EqpId As Short) As Integer

        Dim strTmp As System.Text.StringBuilder
        Dim mlngRet As Integer
        Dim strMSG As String

        Try
            ' �@��A�h���X�̃C�N�E�B�b�v�����gID(mintSelectEqpId)���擾
            mlngRet = GpibSetEquipment(mintDevId, EqpId, Addr, 0, Delim)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
                MsgBox(strTmp.ToString)
                Return (mlngRet)
            End If

            ' �f���~�^�EEOI�̐ݒ�
            mlngRet = GpibSetDelim(EqpId, Delim, Eoi, 0)
            If (mlngRet <> 0) Then                                      ' �G���[ ?
                strTmp = New System.Text.StringBuilder("", 256)
                Call GpibGetErrorString(mlngRet, strTmp)
                MsgBox(strTmp.ToString)
            End If
            Return (mlngRet)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "GpibMaster.funcSetParam() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (-1)                                                 ' Return�l = �ׯ�ߴװ����
        End Try
    End Function
#End Region

End Class
