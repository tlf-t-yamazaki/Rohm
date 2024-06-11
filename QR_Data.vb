'===============================================================================
'   Description  : QR�f�[�^����(���[���a����) V1.18.0.0�A
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'   ReMarks      : �E�F���R���f�U�C���А��Q�����R�[�h���[�_ OPI-2201�p
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports TKY_ALL_SL432HW.My.Resources                                    ' V4.4.0.0-0
Imports LaserFront.Trimmer.DefWin32Fnc                                  ' V6.0.3.0�C

Module QR_Data

#Region "�y�萔/�ϐ��̒�`�z"
    '===========================================================================
    '   �萔/�ϐ��̒�`
    '===========================================================================
    '----- �N���X�I�u�W�F�N�g -----
    Private ObjQR_PortInfo As DllSerialIO.PortInformation = Nothing     ' �V���A���|�[�g���I�u�W�F�N�g(DllSerialIO.dll)
    Private ObjQRDataIO As DllQRDataIO.QRDataIO = Nothing               ' QR�f�[�^���o�̓I�u�W�F�N�g(DllQRDataIO.dll)

    Private N As Integer = 5                                            ' V6.0.3.0_21  4 -> 5
    Public gsQRInfo(N) As String                                        ' QR���\���p�ޯ̧
    Public giSTART(N) As Integer                                        ' �J�n�� 0:�^�C�v, 1:���e��, 2:��R�l(IEC), 3:��R�l(����R�l), 4:Lot No.
    Public giEND(N) As Integer                                          ' �I���� 0:�^�C�v, 1:���e��, 2:��R�l(IEC), 3:��R�l(����R�l), 4:Lot No.
    Public giUse(N) As Integer                                          ' �g�p�� 0:�^�C�v, 1:���e��, 2:��R�l(IEC), 3:��R�l(����R�l), 4:Lot No.

    Public QR_Data As String                                            ' QR���ގ�M�ް�(text)
    Public QR_Read_Flg As Integer = 0                                   ' QR���ޓǂݍ��ݔ���(0)NG (1)OK
    Public strQRLoadFileFullPath As String                              ' QR���ނ���ǂݏo����̧���߽
    Public Const QRDATA_DIR_PATH As String = "C:\TRIMDATA"              ' �f�[�^�t�@�C���t�H���_�[
    Private Const SYSPARAMPATH As String = "C:\TRIM\tky.ini"            ' �V�X�e���p�����[�^�p�X�� ' V6.0.3.0�C

    '----- �t���O -----
    Public QR_Rs_Flag As Integer = 0                                    ' �V���A���|�[�g�I�[�v���t���O(0:�������, 1:����ݍ�)

#End Region

#Region "�y���\�b�h��`�z"
    '---------------------------------------------------------------------------
    '   �V���A������M����
    '---------------------------------------------------------------------------
#Region "RS232C�|�[�g�̃I�[�v��"
    '''=========================================================================
    '''<summary>RS232C�|�[�g�̃I�[�v��</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function QR_Rs232c_Open() As Integer

        Dim r As Integer
        Dim strMSG As String
        Dim PortNo As String                                            ' V6.0.3.0�C

        Try
            ' ��������        
            ' V6.0.3.0�D QR_Read_Flg = 0                                ' QR���ޓǍ��ݔ���(0)NG (1)OK 
            r = SerialErrorCode.rRS_OK                                  ' Return�l = ����
            'If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return (r) '  ���[���a�����łȂ����NOP V6.1.4.0_22
            ObjQRDataIO = New DllQRDataIO.QRDataIO
            ObjQR_PortInfo = New DllSerialIO.PortInformation            ' �V���A���|�[�g���I�u�W�F�N�g����

            ' �|�[�g����ݒ肷��
            '----- V6.0.3.0�C�� -----
            'ObjQR_PortInfo.PortName = "COM4"'V4.0.0.0-79               ' �|�[�g�ԍ�
            'ObjQR_PortInfo.PortName = "COM5"                           ' �|�[�g�ԍ�
            PortNo = GetPrivateProfileString_S("QR_CODE", "COM", SYSPARAMPATH, "COM5")
            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                QR_Read_Flg = 0                                         ' QR���ޓǍ��ݔ���(0)NG (1)OK
                PortNo = GetPrivateProfileString_S("QR_CODE", "RS232C_PORT_NO", "C:\TRIM\tky.ini", "COM2")
            End If
            '----- V6.1.4.0_22�� -----
            ObjQR_PortInfo.PortName = PortNo                            ' �|�[�g�ԍ�
            '----- V6.0.3.0�C�� -----
            ObjQR_PortInfo.BaudRate = 9600                              ' Speed
            ObjQR_PortInfo.Parity = 0                                   ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
            ObjQR_PortInfo.DataBits = 8                                 ' �f�[�^�� = 8 Bit
            ObjQR_PortInfo.StopBits = 1                                 ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = ObjQRDataIO.Serial_Open(ObjQR_PortInfo)                 ' �|�[�g�I�[�v��
            If (r = SerialErrorCode.rRS_OK) Then                        ' ���� ? 
                QR_Rs_Flag = 1                                          ' �׸�=1(����ݍ�)
            End If
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Rs232c_Open() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (SerialErrorCode.rRS_Trap)
        End Try
    End Function
#End Region

#Region "RS232C�|�[�g�̃N���[�Y"
    '''=========================================================================
    '''<summary>RS232C�|�[�g�̃N���[�Y</summary>
    '''<returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function QR_Rs232c_Close() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �|�[�g�N���[�Y
            r = SerialErrorCode.rRS_OK                                  ' Return�l = ����
            If (QR_Rs_Flag = 0) Then Exit Function '                    ' �׸�=0(�������)�Ȃ�NOP
            r = ObjQRDataIO.Serial_Close()                              ' �|�[�g�N���[�Y

            '----- V6.0.3.0_36�� -----
            ObjQR_PortInfo = Nothing                                    ' �V���A���|�[�g���I�u�W�F�N�g���
            ObjQRDataIO = Nothing                                       ' �V���A���h�n�I�u�W�F�N�g���
            QR_Rs_Flag = 0                                              ' �׸�=0(�������)
            '----- V6.0.3.0_36�� -----

            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Rs232c_Close() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = SerialErrorCode.rRS_Trap
        End Try

        ObjQR_PortInfo = Nothing                                        ' �V���A���|�[�g���I�u�W�F�N�g���
        ObjQRDataIO = Nothing                                           ' �V���A���h�n�I�u�W�F�N�g���
        QR_Rs_Flag = 0                                                  ' �׸�=0(�������)
        Return (r)
    End Function
#End Region

#Region "��M����QR�f�[�^���擾����"
    '''=========================================================================
    ''' <summary>��M����QR�f�[�^���擾����</summary>
    ''' <param name="receivedData"></param>
    ''' <returns>cFRS_NORMAL  = QR�f�[�^����M����
    '''          cFRS_ERR_RST = QR�f�[�^����M���ĂȂ��@�@�@�@�@�@�@�@�@</returns>
    '''=========================================================================
    Public Function QR_GetReceiveData(ByRef receivedData As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            '----- V6.0.3.0_35�� -----
            'If (QR_Rs_Flag = 0) Then Exit Function '                    ' �t���O=0(RS232C�������)�Ȃ�NOP
            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
            'If (QR_Rs_Flag = 0) Then Return (cFRS_ERR_RST) '            ' RS232C������݂Ȃ�cFRS_ERR_RST��Ԃ� 
            If (QR_Rs_Flag = 0) And (giQrCodeType = QrCodeType.Rome) Then
                Return (cFRS_ERR_RST)
            End If
            '----- V6.1.4.0_22�� -----
            '----- V6.0.3.0_35�� -----

            '----- V6.1.4.0_22��(KOA EW�aSL432RD�Ή�) -----
            If (giQrCodeType = QrCodeType.KoaEw) Then
                If (QR_Rs_Flag = 0) Then                                    ' �V���A���|�[�g�I�[�v���t���O(0:�������, 1:����ݍ�)
                    '�|�[�g�I�[�v��(QR�f�[�^��M�p)
                    r = QR_Rs232c_Open()
                    If (r <> SerialErrorCode.rRS_OK) Then
                        ' "�V���A���|�[�g�n�o�d�m�G���["
                        strMSG = MSG_136 + "(" + "QR Code Reader " + GetPrivateProfileString_S("QR_CODE", "RS232C_PORT_NO", "C:\TRIM\tky.ini", "COM2") + ")"
                        Call MsgBox(strMSG, vbOKOnly)
                        'Return (ret)
                    End If
                End If
            End If
            '----- V6.1.4.0_22�� -----

            ' ��M����QR�f�[�^���擾����
            r = ObjQRDataIO.GetReceiveData(receivedData)
            If (r <> SerialErrorCode.rRS_OK) Then                       ' ��M�f�[�^�Ȃ� ? 
                Return (cFRS_ERR_RST)
            End If
            QR_Read_Flg = 1                                             ' QR���ޓǂݍ��ݔ���(0)NG (1)OK
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.OnReceiveDataEvent() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "QR���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����"
    '''=========================================================================
    ''' <summary>QR���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="strCheckData">(INP)QR���ޓǍ����ް�</param>
    ''' <returns>�t�@�C���p�X��</returns>
    '''=========================================================================
    Public Function GetQrCodeFileName(ByVal strCheckData As String) As String

        Dim strPosGet As String
        Dim strMakeName(4) As String
        Dim strSetFullPath As String
        Dim s(1) As String
        Dim strMSG As String
        Dim r As Integer        'V4.1.0.0�@
        Dim DspMsg As String    'V4.1.0.0�@

        Try
            If (True = String.IsNullOrEmpty(strCheckData)) Then Return String.Empty ' V4.0.0.0-66

            ' �^�C�v�擾(17-23����)�@�@�@�@�@�@�@�@�@�@�@               ' �t�H���_�� = �^�C�v + ���e�� (��)"UCR01G")
            strPosGet = strCheckData.Substring(giSTART(0) - 1, giUse(0))
            strMakeName(0) = RTrim(strPosGet)                           ' "UCR01"   ����󔒂�����

            ' ���e���擾(46-46����)
            strPosGet = strCheckData.Substring(giSTART(1) - 1, giUse(1))
            strMakeName(1) = RTrim(strPosGet)                           ' "G" 

            ' ��R�l�擾(IEC)(48-52����)                               ' �t�@�C���� = �^�C�v + ���e�� + ��R�l�擾(IEX) (��)"UCR01G1R00.tdc")
            strPosGet = strCheckData.Substring(giSTART(2) - 1, giUse(2))
            strMakeName(2) = RTrim(strPosGet)                           ' "1R00" 

            ' ��R�l(����R�l)�擾(53-64����)  
            strPosGet = strCheckData.Substring(giSTART(3) - 1, giUse(3))
            strMakeName(3) = RTrim(strPosGet)

            ' ���b�gNo.(1-16���� (���b�g�ԍ�(1-14����)-���b�g�ԍ�����(15-16����))  
            strPosGet = strCheckData.Substring(giSTART(4) - 1, giUse(4))
            strMakeName(4) = RTrim(strPosGet)

            ' ���b�gNo.�ҏW(���b�g�ԍ�-���b�g�ԍ�����)
            s(0) = strCheckData.Substring(giSTART(4) - 1, giUse(4) - 2)
            s(0) = RTrim(s(0))
            s(1) = strCheckData.Substring(giSTART(4) + giUse(4) - 2 - 1, 2)
            s(1) = Trim(s(1))
            gsQRInfo(4) = s(0) & "-" & s(1)                             ' ���b�gNo.

            '----- V6.0.3.0_21 �� -----               ��
            ' ���b�g���������
            If (0 < giSTART(5)) AndAlso (0 <= giUse(5)) Then            ' V6.0.3.0_23
                strPosGet = strCheckData.Substring(giSTART(5) - 1, giUse(5))
            Else
                strPosGet = "0"
            End If
            Dim n As Integer
            If (Integer.TryParse(strPosGet, n)) Then                    ' TryParse�͋󔒂��܂�ł����Ȃ�
                FormLotEnd.Input = n
                gsQRInfo(5) = n.ToString("0")
            Else
                n = (-1)    ' ���l�Ƃ��Ďg�p�ł��Ȃ�
                gsQRInfo(5) = """" & strPosGet & """"
            End If            
            '----- V6.0.3.0_21 �� -----

            ' �\����ɐݒ�
            gsQRInfo(0) = strMakeName(0)                                ' �^�C�v
            gsQRInfo(1) = strMakeName(1)                                ' ���e��
            gsQRInfo(2) = strMakeName(2)                                ' ��R�l(IEC)  
            gsQRInfo(3) = strMakeName(3)                                ' ��R�l(����R�l)

            ' �t�@�C���p�X�� = �t�H���_��+�t�@�C����
            strSetFullPath = QRDATA_DIR_PATH & "\" & strMakeName(0) & strMakeName(1) & "\" & strMakeName(0) & strMakeName(1) & strMakeName(2) & ".tdc"
            '----- V4.0.0.0�H�� -----
            If (giMachineKd = MACHINE_KD_RS) Then                       ' SL436S ?
                strSetFullPath = QRDATA_DIR_PATH & "\" & strMakeName(0) & strMakeName(1) & "\" & strMakeName(0) & strMakeName(1) & strMakeName(2) & ".tdcs"
            End If
            '----- V4.0.0.0�H�� -----

            ' �t�@�C�������݂��邩�`�F�b�N����
            If (System.IO.File.Exists(strSetFullPath) = False) Then
                strMSG = "File Not Exist " + strSetFullPath
                Call Form1.Z_PRINT(strMSG)
                ' --- V4.1.0.0�@��-----------------------------------------------------
                DspMsg = strSetFullPath
                ' "�p�q�R�[�h�ɑΉ������g���~���O�f�[�^������܂���B","�t�@�C�����m�F���Ă��������B",""
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH48, MSG_SPRASH49, DspMsg, System.Drawing.Color.Blue, System.Drawing.Color.Blue, System.Drawing.Color.Blue)
                ' --- V4.1.0.0�@��-----------------------------------------------------
                strSetFullPath = ""
                '----- V6.0.3.0_21�� -----         ��
                FormLotEnd.Input = 0

            ElseIf (n < 0) Then
                FormLotEnd.Input = 0

                ' "���b�g����������𐔒l�Ƃ��ĔF���ł��܂���B", "0 �Ƃ��ď������܂��B" 'V6.0.3.0_21
                r = Sub_CallFrmMsgDisp(Form1.System1, cGMODE_MSG_DSP, cFRS_ERR_START, True, _
                        MSG_SPRASH72, gsQRInfo(5), MSG_SPRASH73, Color.Blue, Color.Blue, Color.Blue)

                gsQRInfo(5) &= " : NaN -> 0"
                Form1.Z_PRINT(gsQRInfo(5))
            Else
                ' DO NOTHING
                '----- V6.0.3.0_21�� -----            ��
            End If

            Return (strSetFullPath)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "QR_Data.GetQrCodeFileName() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region

#Region "QR�f�[�^�Ŏw��̃t�@�C�������[�h����"
    '''=========================================================================
    ''' <summary>QR�f�[�^�Ŏw��̃t�@�C�������[�h����</summary>
    ''' <param name="pPath">(INP)�t�@�C���p�X��</param>
    '''=========================================================================
    Public Sub DataLoadQR(ByRef pPath As String)

        Dim r As Integer
        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strMSG As String

        Try
            ' ��������
            Call SetMousePointer(Form1, True)                           ' �����v�\��(mouse pointer)

            ' �t�@�C�����[�h
            r = Form1.Sub_FileLoad(pPath)
            If (r <> cFRS_NORMAL) Then                                  ' �t�@�C�����[�h�G���[ ?(���G���[���b�Z�[�W�͕\���ς�) 
                Exit Sub
            End If

            ' �I������
            Call Form1.ClearCounter(1)                                  ' ���Y�Ǘ��f�[�^�N���A
            Call ClrTrimPrnData()                                       ' �g���~���O���ʈ�����ڏ����� 
            Call QR_Info_Disp(1)                                        ' QR�R�[�h����\������

            'Call SetMousePointer(Form1, False)                          ' V6.0.3.0_22 �����v����(mouse pointer)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.DataLoadQR() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)

        Finally                                                         ' V6.0.3.0_22
            SetMousePointer(Form1, False)                               ' �����v����(mouse pointer)

        End Try
    End Sub
#End Region

#Region "QR�f�[�^�̃I�t�Z�b�g�ʒu���V�X�p������擾����"
    '''=========================================================================
    ''' <summary>QR�f�[�^�̃I�t�Z�b�g�ʒu���V�X�p������擾����</summary>
    ''' <param name="sPath">(INP)�V�X�e���p�����[�^�p�X��</param>
    '''=========================================================================
    Public Sub GetSysPrm_QR_DataOfs(ByRef sPath As String)

        Dim strMSG As String

        Try
            ' QR�f�[�^�̃I�t�Z�b�g�ʒu���V�X�p������擾����
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '  ���[���a�����łȂ����NOP

            ' QR�R�[�h(�^�C�v)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_ST", giSTART(0))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_END", giEND(0))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "TYPE_USE", giUse(0))

            ' QR�R�[�h(���e��)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_ST", giSTART(1))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_END", giEND(1))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "ALLO_USE", giUse(1))

            ' QR�R�[�h(��R�lIEC)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_ST", giSTART(2))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_END", giEND(2))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RIEC_USE", giUse(2))

            ' QR�R�[�h(��R�lTURE)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_ST", giSTART(3))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_END", giEND(3))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "RTRU_USE", giUse(3))

            ' QR�R�[�h(Lot No.)
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_ST", giSTART(4))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_END", giEND(4))
            Call Get_SystemParameterShort(sPath, "QR_CODE", "LOTN_USE", giUse(4))

            '----- V6.0.3.0_21�� -----            ��
            ' ���b�g���������
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_ST", giSTART(5))
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_END", giEND(5))
            Get_SystemParameterShort(sPath, "QR_CODE", "LTIN_USE", giUse(5))

            ' ���e�����
            Get_SystemParameterShort(sPath, "QR_CODE", "ALB_NUM", FormLotEnd.Allowable)
            '----- V6.0.3.0_21�� -----          ��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.GetSysPrm_QR_DataOfs() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "QR�R�[�h����\������"
    '''=========================================================================
    ''' <summary>QR�R�[�h����\������</summary>
    ''' <param name="Md">(INP)���[�h(0=������, 1=�\��)</param>
    '''=========================================================================
    Public Sub QR_Info_Disp(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' QR�f�[�^�̃I�t�Z�b�g�ʒu���V�X�p������擾����
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '  ���[���a�����łȂ����NOP

            ' ������
            If (Md = 0) Then
                gsQRInfo(0) = ""
                gsQRInfo(1) = ""
                gsQRInfo(2) = ""
                gsQRInfo(3) = ""
                gsQRInfo(4) = ""
                gsQRInfo(5) = ""                                    ' V6.0.3.0_21
            End If

            ' QR�R�[�h����\������
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    Form1.lblQRData.Text = " �^�C�v�@�@�@�@�@�@�F" & gsQRInfo(0) & vbCrLf & _
            '                            " ���e���@�@�@�@�@�@�F" & gsQRInfo(1) & vbCrLf & _
            '                            " ��R�l�i�h�d�b�j�@�F" & gsQRInfo(2) & vbCrLf & _
            '                            " ��R�l�i����R�l�j�F" & gsQRInfo(3) & vbCrLf & _
            '                            " ���b�g�m���D�@�@�@�F" & gsQRInfo(4)
            'Else
            '    Form1.lblQRData.Text = " Type               :" & gsQRInfo(0) & vbCrLf & _
            '                             " Allowance          :" & gsQRInfo(1) & vbCrLf & _
            '                             " Res Value(IEC)     :" & gsQRInfo(2) & vbCrLf & _
            '                             " Res Value(True Res):" & gsQRInfo(3) & vbCrLf & _
            '                             " Lot No.            :" & gsQRInfo(4)
            'End If
            ''If (gSysPrm.stTMN.giMsgTyp = 0) Then
            ''    Form1.lblQRData.Text = "[QR�R�[�h���]" & vbCrLf & _
            ''                            " �^�C�v�@�@�@�@�@�@�F" & gsQRInfo(0) & vbCrLf & _
            ''                            " ���e���@�@�@�@�@�@�F" & gsQRInfo(1) & vbCrLf & _
            ''                            " ��R�l�i�h�d�b�j�@�F" & gsQRInfo(2) & vbCrLf & _
            ''                            " ��R�l�i����R�l�j�F" & gsQRInfo(3) & vbCrLf & _
            ''                            " ���b�g�m���D�@�@�@�F" & gsQRInfo(4) & vbCrLf &
            ''                            " ���b�g����������F" & gsQRInfo(5)     ' V6.0.3.0_21
            ''Else
            ''    Form1.lblQRData.Text = "[QR CODE INFORMATION]" & vbCrLf & _
            ''                             " Type               :" & gsQRInfo(0) & vbCrLf & _
            ''                             " Allowance          :" & gsQRInfo(1) & vbCrLf & _
            ''                             " Res Value(IEC)     :" & gsQRInfo(2) & vbCrLf & _
            ''                             " Res Value(True Res):" & gsQRInfo(3) & vbCrLf & _
            ''                             " Lot No.            :" & gsQRInfo(4) & vbCrLf &
            ''                             " Lot Input          :" & gsQRInfo(5)    ' V6.0.3.0_21
            ''End If
            Form1.lblQRData.Text = QR_Data_001 & gsQRInfo(0) & vbCrLf & _
                                   QR_Data_002 & gsQRInfo(1) & vbCrLf & _
                                   QR_Data_003 & gsQRInfo(2) & vbCrLf & _
                                   QR_Data_004 & gsQRInfo(3) & vbCrLf & _
                                   QR_Data_005 & gsQRInfo(4) & vbCrLf &
                                   QR_Data_006 & gsQRInfo(5)            ' V6.0.3.0_21
            Form1.lblQRData.Refresh()

            ' ������ڂɃ^�C�v�ƃ��b�gNo.��ݒ肷��
            stPRT_ROHM.Prot_Typ = gsQRInfo(0)                           ' �^�C�v
            stPRT_ROHM.Lot_No = gsQRInfo(4)                             ' ���b�gNo.

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Data.QR_Info_Disp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region
End Module
