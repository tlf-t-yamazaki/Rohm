Attribute VB_Name = "Rs232c_FL"
'==============================================================================
'
'   DESCRIPTION:    �g���}�[���H������FL�����RS232C�ő���M����
'                   (C#�ō쐬���ꂽ�DllFLCom.dll����g�p)
'                   ���Ώۋ@��@FL�pFPGA�ݒ�p
'                     �ʐM����       : ��������������d�ʐM
'                     �{�[���[�g     : 38,400BPS
'                     �L�����N�^�[�� : 8 Bit
'                     �p���e�B       : �Ȃ�
'                     �X�g�b�v       : 1 BIT
'                     �f���~�^�R�[�h : CR
'
'                   ����M�f�[�^�`��
'                   1. ���M�f�[�^�`��(PC�@���� FL)
'                   -------------------------------------------
'                   |�R�}���h��(2) | �f�[�^(4)        | CR(1) |
'                   |A�`P��ASCII   |0�`9,a�`f��ASCII  |       |
'                   -------------------------------------------
'                   2. �����v���f�[�^�`��(PC�@�� FL)
'                   -----------------------------------
'                   |�R�}���h��(2) | ذ��(1) |�@CR(1) |
'                   |A�`P��ASCII   |  r      |�@      |
'                   -----------------------------------
'
'==============================================================================
Option Explicit
'==============================================================================
'   �ϐ���`
'==============================================================================
'----- �|�[�g��� -----
Public Type ComInfo
    PortName    As String                               ' �q�r�Q�R�Q�b�|�[�g�ԍ�
    BaudRate    As Long                                 ' �`���X�s�[�h
    Parity      As Integer                              ' �p���e�B(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
    DataBits    As Integer                              ' �f�[�^�[��
    StopBits    As Integer                              ' �X�g�b�v�r�b�g��
End Type

'----- �N���X�I�u�W�F�N�g -----
Private ObjPortInfo     As DllSerialIO.PortInformation  ' �V���A���|�[�g���I�u�W�F�N(DllSerialIO.dll)
Private ObjFLCom        As DllFLCom.FLComIO             ' �g���}�[���H��������M�I�u�W�F�N�g(DllFLCom.dll)
                                                        
                                                        ' �o�[�W�������N���X
Private ObjPortCVer     As DllSerialIO.VersionInformation   ' DllSerialIO.dll
Private ObjFLXMLVer     As DllCndXMLIO.VersionInformation   ' DllCndXMLIO.dll
Private ObjFLComVer     As DllFLCom.VersionInformation      ' DllFLCom.dll

Private Rs_Flag         As Integer                      ' �׸�(0:�������, 1:����ݍ�)

'----- �g���}�[���H���� -----
Public Const MAX_BANK_NUM   As Integer = 32             ' �ő���H������(0-31)
Public Const MAX_STEG_NUM   As Integer = 20             ' STEG�g�`�ő�l(1-20)
Public Const MAX_CURR_VAL   As Integer = 8500           ' �ő�d���l(mA)
Public Const MIN_CURR_VAL   As Integer = 1              ' �ŏ��d���l(mA)
'Public Const MAX_FREQ_VAL   As Integer = 50            ' �ő���g��(KHz)
Public Const MAX_FREQ_VAL   As Integer = 100            ' �ő���g��(KHz)
Public Const MIN_FREQ_VAL   As Integer = 1              ' �ŏ����g��(KHz)

Public Type TrimCondInfo                                ' �g���}�[���H�����`����`
    Curr(MAX_BANK_NUM)      As Long                     ' �d���l(mA)
    Freq(MAX_BANK_NUM)      As Double                   ' ���g��(KHz)
    Steg(MAX_BANK_NUM)      As Long                     ' STEG�g�`
End Type

'^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
'   �G���[�R�[�h(C#�ō쐬���ꂽdll���Ԃ��Ă������)
'^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Public Enum SerialErrorCode
'----- 1-18��DllSerialIO�Ŏg�p -----
    rRS_OK = 0                                          '  0:����
    rRS_ReadTimeout                                     '  1:���[�h�^�C���A�E�g
    rRS_WriteTimeout                                    '  2:���C�g�^�C���A�E�g
    rRS_RespomseTimeout                                 '  3:�����^�C���A�E�g
    rRS_FailOpen                                        '  4:�ر��߰ĵ���ݎ��s
    rRS_FailClose                                       '  5:�ر��߰ĸ۰�ގ��s
    rRS_FailInit                                        '  6:�ر��߰ď��������s
    rRS_SerialErrorFrame                                '  7:H/W���ڰѴװ���o
    rRS_SerialErrorOverrun                              '  8:�����ޯ̧�̵��ް�ݔ���
    rRS_SerialErrorRXOver                               '  9:�����ޯ̧�̵��ް�۰����
    rRS_SerialErrorRXParity                             ' 10:H/W�����è�װ����
    rRS_SerialErrorTXFull                               ' 11:���ع���݂͕����𑗐M���悤�Ƃ������o���ޯ̧����t
    rRS_InvalidSerialProtInfo                           ' 12:�V���A���|�[�g���s��
    rRS_InvalidValue                                    ' 13:�����ȃf�[�^
    rRS_FailSerialRead                                  ' 14:�ر��߰Ă���̓Ǎ����s
    rRS_FailSerialWrite                                 ' 15:�ر��߰Ăւ̏������s
    rRS_NotOpen                                         ' 16:�ر��߰Ă�����݂��Ă��Ȃ�
    rRS_Exception                                       ' 17:��O
    
    '----- �ȍ~�͓��֐��Ŏg�p -----
    rRS_FLCND_NONE = 101                                ' 101:���H�����̐ݒ�Ȃ�
    rRS_FLCND_XMLNONE = 102                             ' 102:���H�����t�@�C�������݂��Ȃ�
    rRS_FLCND_XMLREADERR = 103                          ' 103:���H�����t�@�C�����[�h�G���[
    rRS_FLCND_XMLWRITERR = 104                          ' 104:���H�����t�@�C�����C�g�G���[
    rRS_FLCND_SNDERR = 105                              ' 105:���H�������M�G���[
    rRS_FLCND_RCVERR = 106                              ' 106:���H������M�G���[

'----- �ȍ~��DllFLCom�Ŏg�p -----
    rRS_CndNum = 900                                    '900:���H�����ԍ��G���[
    rRS_Trap = 999                                      '999:�g���b�v�G���[����
    
End Enum

'==============================================================================
'  �@�\: RS232C�|�[�g�̃I�[�v��
'�@����: pstCom (INP): �|�[�g���
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function Rs232cFL_Open(pstCom As ComInfo) As Long

    On Error GoTo STP_ERR
    Dim r           As Long

    ' ��������
    Rs232cFL_Open = rRS_OK                              ' Return�l = ����
    Set ObjPortInfo = New DllSerialIO.PortInformation   ' �߰ď���޼ު�Đ���
    Set ObjFLCom = New DllFLCom.FLComIO                 ' �g���}�[���H��������M��޼ު�Đ���
            
    ' �|�[�g����ݒ肷��
    ObjPortInfo.PortName = pstCom.PortName              ' �|�[�g�ԍ�
    ObjPortInfo.BaudRate = pstCom.BaudRate              ' Speed
    ObjPortInfo.Parity = pstCom.Parity                  ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
    ObjPortInfo.DataBits = pstCom.DataBits              ' Char Data
    ObjPortInfo.StopBits = pstCom.StopBits              ' Stop Bit
    
    ' �|�[�g�I�[�v��
    r = ObjFLCom.Serial_Open(ObjPortInfo)               ' �|�[�g�I�[�v��
    If (r <> rRS_OK) Then
        Rs232cFL_Open = r                               ' Return�l�ݒ�
    End If
    Rs_Flag = 1                                         ' �׸�=1(����ݍ�)
    Exit Function
    
    ' �g���b�v�G���[������
STP_ERR:
    mFncName = "Rs232c_FL.Rs232cFL_Open()"
    Call gObjUtl.SystemErrLog(mModName, mFncName, err.Number, err.Description)
    err.Clear
    Rs232cFL_Open = rRS_Trap                            ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: RS232C�|�[�g�̃N���[�Y
'�@����: �Ȃ�
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function Rs232cFL_Close() As Long

    On Error GoTo STP_ERR
    Dim r           As Long
     
    ' �|�[�g�N���[�Y
    Rs232cFL_Close = rRS_OK                             ' Return�l = ����
    If (Rs_Flag = 0) Then Exit Function                 ' �׸�=0(�������)�Ȃ�NOP
    r = ObjFLCom.Serial_Close()                         ' �|�[�g�N���[�Y
    Rs232cFL_Close = r                                  ' Return�l�ݒ�
    Rs_Flag = 0                                         ' �׸�=0(�������)

STP_END:
    Set ObjPortInfo = Nothing                           ' �߰ď���޼ު�ĉ��
    Set ObjFLCom = Nothing                              ' �g���}�[���H��������M��޼ު�ĉ��
    Exit Function

STP_ERR:
    Rs232cFL_Close = rRS_Trap                           ' Return�l = �g���b�v�G���[����
    GoTo STP_END
    
End Function

'==============================================================================
'  �@�\: �V���A���|�[�g�փg���}�[���H�������ʂɑ��M����
'  ����: CndNum (INP): �����ԍ�
'        Curr   (INP): �d���l
'        Freq   (INP): ���g��
'        Steg   (INP): STEG�g�`
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function RsSendBankInfo(CndNum As Long, Curr As Long, Freq As Double, Steg As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long
    Dim dblWK       As Long

    ' ��������
    On Error GoTo STP_ERR
    RsSendBankInfo = rRS_OK                             ' Return�l = ����
    If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
        RsSendBankInfo = rRS_NotOpen                    ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
        Exit Function
    End If
        
    ' ���g��(KHz)���J�Ԃ��Ԋu�ɕϊ�����
    ' �J�Ԃ��ԊuN = �J�Ԃ�����/200ns ��)10KHz�̏ꍇ�́A500(0x01f4)�𑗐M����B
    If (Freq <= 0) Then
        wkFreq = 0
    Else
        dblWK = 1000000 / 200 / Freq                    '###803�@(OcxSystem)
        wkFreq = dblWK                                  '###803�@(OcxSystem)
'       wkFreq = 1000000 / 200 / Freq                   '###803�@(OcxSystem)
    End If
    
    ' �g���}�[���H�������M
    r = ObjFLCom.Serial_SendBankInfo(CndNum, Curr, wkFreq, Steg)
    If (r <> rRS_OK) Then                               ' ���M�G���[ ?
        RsSendBankInfo = r                              ' Return�l�ݒ�
        Exit Function
    End If
    Exit Function

STP_ERR:
    RsSendBankInfo = rRS_Trap                           ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: �V���A���|�[�g�փg���}�[���H�������ꊇ�ő��M����
'  ����: Curr() (INP): �d���l
'        Freq() (INP): ���g��
'        Steg() (INP): STEG�g�`
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function RsSendBankALL(Curr() As Long, Freq() As Double, Steg() As Long) As Long

    Dim r           As Long
    Dim i           As Long

    ' ��������
    On Error GoTo STP_ERR
    RsSendBankALL = rRS_OK                              ' Return�l = ����
    If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
        RsSendBankALL = rRS_NotOpen                     ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
        Exit Function
    End If
        
    ' �V���A���|�[�g�փg���}�[���H�����𑗐M����
    For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H�����������M����
        r = RsSendBankInfo(i, Curr(i), Freq(i), Steg(i))
        If (r <> rRS_OK) Then                           ' ���M�G���[ ?
            RsSendBankALL = r                           ' Return�l�ݒ�
            Exit Function
        End If
    Next i
    Exit Function

STP_ERR:
    RsSendBankALL = rRS_Trap                            ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: �V���A���|�[�g����g���}�[���H�������ʂɎ�M����
'  ����: CndNum (INP): �����ԍ�
'        Curr   (OUT): �d���l(mA)
'        Freq   (OUT): ���g��(KHz)
'        Steg   (OUT): STEG�g�`(0-15)
'        TimeOut(INP): �����҃^�C�}�l(ms)
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function RsReceiveBankInfo(CndNum As Long, Curr As Long, Freq As Double, Steg As Long, timeout As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long
    Dim dblWK       As Double
    Dim dblWK2      As Double
    Dim strMSG      As String

    ' ��������
    On Error GoTo STP_ERR
    RsReceiveBankInfo = rRS_OK                          ' Return�l = ����
    If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
        RsReceiveBankInfo = rRS_NotOpen                 ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
        Exit Function
    End If
        
    ' �g���}�[���H������M
    r = ObjFLCom.Serial_ReceiveBankInfo(CndNum, Curr, wkFreq, Steg, timeout)
    If (r <> rRS_OK) Then                               ' ���M�G���[ ?
        RsReceiveBankInfo = r                           ' Return�l�ݒ�
        Exit Function
    End If
    
    ' �J�Ԃ��Ԋu�����g��(KHz)�ɕϊ�����
    If (wkFreq <= 0) Then
        Freq = 0
    Else
        '###803�@(OcxSystem)��
        dblWK = wkFreq * 200
        dblWK2 = 1000000
        Freq = dblWK2 / dblWK
        strMSG = Format((Freq * 10), "0.0")             ' �����_�Q�ʈȉ���؂�̂Ă�
        Freq = CDbl(strMSG)
        Freq = Freq / 10
        '###803�@(OcxSystem)��
    
'        '###801�C(OcxLaser) ��       Freq = 1000000 / (wkFreq * 200)
'        Freq = 1000000 / (wkFreq * 200)
'        wkFreq = Fix(10 * Freq)
'        Freq = wkFreq / 10
'        '###801�C(OcxLaser) ��
        
    End If
    Exit Function

STP_ERR:
    RsReceiveBankInfo = rRS_Trap                        ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
'  ����: Curr   (OUT): �d���l(mA)
'        Freq   (OUT): ���g��(KHz)
'        Steg   (OUT): STEG�g�`(0-15)
'        TimeOut(INP): �����҃^�C�}�l(ms)
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function RsReceiveBankALL(Curr() As Long, Freq() As Double, Steg() As Long, timeout As Long) As Long

    Dim r           As Long
    Dim i           As Long

    ' ��������
    On Error GoTo STP_ERR
    RsReceiveBankALL = rRS_OK                           ' Return�l = ����
    If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
        RsReceiveBankALL = rRS_NotOpen                  ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
        Exit Function
    End If
    
    ' �V���A���|�[�g����g���}�[���H�������ꊇ�Ŏ�M����
    For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H����������M����
        r = RsReceiveBankInfo(i, Curr(i), Freq(i), Steg(i), timeout)
        If (r <> rRS_OK) Then                           ' �G���[ ?
            RsReceiveBankALL = r                        ' Return�l�ݒ�
            Exit Function
        End If
    Next i
    
     ' FL���̐ݒ肪���邩�m�F����
    For i = 0 To (MAX_BANK_NUM - 1)                     ' �ő���H����������M����
        If (Curr(i) <> 0) Then                          ' �d���l�ݒ�0�ȊO������������FL���̐ݒ肪����Ɣ��f����
            RsReceiveBankALL = rRS_OK                   ' Return�l = ����
            Exit Function
        End If
    Next i
    RsReceiveBankALL = rRS_FLCND_NONE                   ' Return�l = ���H�����̐ݒ�Ȃ�
   
    Exit Function

STP_ERR:
    RsReceiveBankALL = rRS_Trap                         ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: �V���A���|�[�g����G���[������M����
'  ����: ErrInf (OUT): �G���[���
'        TimeOut(INP): �����҃^�C�}�l(ms)
'�@�ߒl: 0     = ����
'        0�ȊO = �G���[
'==============================================================================
Public Function Rs232cFL_ReceiveErrInfo(ErrInf As Long, timeout As Long) As Long

    Dim r           As Long
    Dim wkFreq      As Long

    ' ��������
    On Error GoTo STP_ERR
    Rs232cFL_ReceiveErrInfo = rRS_OK                    ' Return�l = ����
    If (Rs_Flag = 0) Then                               ' �׸�=0(�������) ?
        Rs232cFL_ReceiveErrInfo = rRS_NotOpen           ' Return�l = �ر��߰Ă�����݂��Ă��Ȃ�
        Exit Function
    End If
        
    ' �G���[����M
    r = ObjFLCom.Serial_ReceiveErrInfo(ErrInf, timeout)
    If (r <> rRS_OK) Then                               ' ��M�G���[ ?
        Rs232cFL_ReceiveErrInfo = r                     ' Return�l�ݒ�
        Exit Function
    End If
    
    Exit Function

STP_ERR:
    Rs232cFL_ReceiveErrInfo = rRS_Trap                  ' Return�l = �g���b�v�G���[����

End Function

'==============================================================================
'  �@�\: �o�[�W�����̎擾
'�@����: strVER (OUT): strVER (0) = DllSerialIO.dll�̃o�[�W����
'�@�@�@�@�@�@�@�@�@�@: strVER (1) = DllCndXMLIO.dll�̃o�[�W����
'�@�@�@�@�@�@�@�@�@�@: strVER (2) = DllFLCom.dll�̃o�[�W����
'�@�ߒl: �Ȃ�
'==============================================================================
Public Sub Rs232cFL_GetVersion(strVER() As String)

    On Error GoTo STP_ERR
    Dim iMajor      As Long                             ' Major Version
    Dim iMinor      As Long                             ' Minor Version
    Dim iBNum       As Long                             ' Build Number
    Dim iRev        As Long                             ' Revision
    
    ' �o�[�W�������N���X�I�u�W�F�N�g����
    Set ObjPortCVer = New DllSerialIO.VersionInformation
    Set ObjFLXMLVer = New DllCndXMLIO.VersionInformation
    Set ObjFLComVer = New DllFLCom.VersionInformation
    
    ' �o�[�W�����̎擾("Vx.x.x.x"�̌`���ŕԂ�)
    Call ObjPortCVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(0) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    Call ObjFLXMLVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(1) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    Call ObjFLComVer.GetVersion(iMajor, iMinor, iBNum, iRev)
    strVER(2) = "V" + Format(iMajor, "0") + "." + Format(iMinor, "0") + "." + Format(iBNum, "0") + "." + Format(iRev, "0")
    
    ' �o�[�W�������N���X�I�u�W�F�N�g�J��
    Set ObjPortCVer = Nothing
    Set ObjFLXMLVer = Nothing
    Set ObjFLComVer = Nothing

STP_ERR:

End Sub

