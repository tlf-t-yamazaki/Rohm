'===============================================================================
'   Description  : �o�[�R�[�h�f�[�^����(���z�Гa����) V1.23.0.0�@
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'   ReMarks      : Cino�А�F460GV USB(RS232C�C���^�[�t�F�[�X))
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports System.Collections.Generic      'V5.0.0.9�N
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module BarCode_Data

#Region "�y�萔/�ϐ��̒�`�z"
    '===========================================================================
    '   �萔/�ϐ��̒�`
    '===========================================================================
    '----- �N���X�I�u�W�F�N�g -----
    Private ObjBC_PortInfo As DllSerialIO.PortInformation = Nothing     ' �V���A���|�[�g���I�u�W�F�N�g(DllSerialIO.dll���g�p)
    Private ObjBCDataIO As DllQRDataIO.QRDataIO = Nothing               ' �o�[�R�[�h�f�[�^���o�̓I�u�W�F�N�g(DllQRDataIO.dll���g�p)

    '----- �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C���̃f�[�^�C���f�b�N�X -----
    Private Const BC_IDX_MAX As Integer = 32                            ' �f�[�^���ڂ̍ő吔
    Private Const BC_IDX_TYPE As Integer = 0                            ' �i�� 
    Private Const BC_IDX_OPTA As Integer = 1                            ' �I�v�V�����`
    Private Const BC_IDX_OPTB As Integer = 2                            ' �I�v�V�����a
    Private Const BC_IDX_RVAL As Integer = 3                            ' ��R�l
    Private Const BC_IDX_BCOD As Integer = 4                            ' �o�[�R�[�h
    Private Const BC_IDX_DATA As Integer = 5                            ' �g���~���O�f�[�^��
    Private Const BC_IDX_BCOD2 As Integer = 6                           ' �o�[�R�[�h�łQ��ڂɓǍ��񂾃f�[�^  'V5.0.0.9�R

    '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
    ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C���̃f�[�^�C���f�b�N�X
    Private Const BC2_IDX_MAX As Integer = 128                          ' �f�[�^���ڂ̍ő吔
    Private Const BC2_IDX_BCOD As Integer = 0                           ' �o�[�R�[�h
    Private Const BC2_IDX_DATA As Integer = 1                           ' �g���~���O�f�[�^��

    ' SizeCode.ini�̍ő吔
    Private Const MAX_SizeCode As Integer = 32

    ' ��T�C�Y�p�\����(SizeCode.ini����荞��)
    Public Structure SizeCode_Data_Info
        Dim SizeBar As String                                           ' ��T�C�Y(�o�[�R�[�h��3,4������)
        Dim SizePrt As String                                           ' ��T�C�Y(�\���p)
    End Structure
    Public StSizeCode(MAX_SizeCode) As SizeCode_Data_Info               ' 0 RG

    ' SizeCode.ini�̃p�X��
    Public Const PATH_SIZCDE_DATA As String = "C:\TRIM\SizeCode.ini"
    '----- V4.11.0.0�A�� -----

    '----- ���̑� -----
    Public BC_Data As String                                            ' �o�[�R�[�h��M�f�[�^(text)
    Public BC_Read_Flg As Integer = 0                                   ' �o�[�R�[�h�ǂݍ��ݔ���(0)NG (1)OK
    Public strBCConvFileFullPath As String                              ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή��t�@�C����(�T�[�o)
    Public strBCLoadFileFullPath As String                              ' �o�[�R�[�h����ǂݏo�����t�@�C���p�X��
    'V5.0.0.9�R    Public gsBCInfo(5) As String                                        ' �o�[�R�[�h���\���p�o�b�t�@
    Public gsBCInfo(6) As String                                        ' �o�[�R�[�h���\���p�o�b�t�@    'V5.0.0.9�R

    'V5.0.0.9�N                  ��
    Public Type As BarcodeType
    Public SubStr As New List(Of Tuple(Of Integer, Integer))

    Public Enum BarcodeType As Integer
        None = 0
        Walsin = 1
        Taiyo = 2
        Standard = 3
    End Enum
    'V5.0.0.9�N                  ��

    '----- �t���O -----
    Public BC_Rs_Flag As Integer = 0                                    ' �V���A���|�[�g�I�[�v���t���O(0:�������, 1:����ݍ�)

    ' �o�[�R�[�h�œǍ��񂾓��e�̕ۑ��p      'V5.0.0.9�R
    Friend BC_ReadDataFirst As String                                   ' �o�[�R�[�h�P��ڂœǍ��񂾃f�[�^�ۑ��p
    Friend BC_ReadDataSecound As String                                 ' �o�[�R�[�h�Q��ڂœǍ��񂾃f�[�^�ۑ��p
    Friend BC_ReadCount As Integer = 0                                  ' �o�[�R�[�h�Ǎ��݉� 

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
    Public Function BC_Rs232c_Open() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            BC_Read_Flg = 0                                             ' �o�[�R�[�h�Ǎ��ݔ���(0)NG (1)OK
            r = SerialErrorCode.rRS_OK                                  ' Return�l = ����
            '----- V4.11.0.0�A�� -----
            ' ���z�Гa����/WALSIN�a�����łȂ����NOP
            'If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) Then Return (r) ' ���z�Гa�����łȂ����NOP
            'V5.0.0.9�N If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) And (gSysPrm.stCTM.giSPECIAL <> customWALSIN) Then
            If (BarcodeType.None = BarCode_Data.Type) Then              'V5.0.0.9�N �o�[�R�[�h�Ȃ�
                Return (r)
            End If
            '----- V4.11.0.0�A�� -----
            ObjBCDataIO = New DllQRDataIO.QRDataIO
            ObjBC_PortInfo = New DllSerialIO.PortInformation            ' �V���A���|�[�g���I�u�W�F�N�g����

            ' �|�[�g����ݒ肷��
            ObjBC_PortInfo.PortName = gsComPort                         ' �|�[�g�ԍ�

            ObjBC_PortInfo.BaudRate = 9600                              ' Speed
            ObjBC_PortInfo.Parity = 0                                   ' Parity(0:None, 1:Odd, 2:Even, 3:Mark, 4:Space)
            ObjBC_PortInfo.DataBits = 8                                 ' �f�[�^�� = 8 Bit
            ObjBC_PortInfo.StopBits = 1                                 ' Stop Bit = 1 Bit

            ' �|�[�g�I�[�v��
            r = ObjBCDataIO.Serial_Open(ObjBC_PortInfo)                 ' �|�[�g�I�[�v��
            If (r = SerialErrorCode.rRS_OK) Then                        ' ���� ? 
                BC_Rs_Flag = 1                                          ' �׸�=1(����ݍ�)
            End If
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Rs232c_Open() TRAP ERROR = " + ex.Message
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
    Public Function BC_Rs232c_Close() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' �|�[�g�N���[�Y
            r = SerialErrorCode.rRS_OK                                  ' Return�l = ����
            If (BC_Rs_Flag = 0) Then Exit Function '                    ' �׸�=0(�������)�Ȃ�NOP
            r = ObjBCDataIO.Serial_Close()                              ' �|�[�g�N���[�Y
            'V5.0.0.9�N            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Rs232c_Close() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            r = SerialErrorCode.rRS_Trap
        End Try

        ObjBC_PortInfo = Nothing                                        ' �V���A���|�[�g���I�u�W�F�N�g���
        ObjBCDataIO = Nothing                                           ' �V���A���h�n�I�u�W�F�N�g���
        BC_Rs_Flag = 0                                                  ' �׸�=0(�������)
        Return (r)
    End Function
#End Region

#Region "��M�����o�[�R�[�h���擾����"
    '''=========================================================================
    ''' <summary>��M�����o�[�R�[�h���擾����</summary>
    ''' <param name="receivedData"></param>
    ''' <returns>cFRS_NORMAL  = QR�f�[�^����M����
    '''          cFRS_ERR_RST = QR�f�[�^����M���ĂȂ��@�@�@�@�@�@�@�@�@</returns>
    '''=========================================================================
    Public Function BC_GetReceiveData(ByRef receivedData As String) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ��������
            If (BC_Rs_Flag = 0) Then Exit Function '                    ' �t���O=0(RS232C�������)�Ȃ�NOP

            ' ��M�����o�[�R�[�h���擾����
            r = ObjBCDataIO.GetReceiveData(receivedData)
            If (r <> SerialErrorCode.rRS_OK) Then                       ' ��M�f�[�^�Ȃ� ? 
                Return (cFRS_ERR_RST)
            End If
            BC_Read_Flg = 1                                             ' �o�[�R�[�h�ǂݍ��ݔ���(0)NG (1)OK
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_GetReceiveData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����"
    '''=========================================================================
    ''' <summary>�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="strBarCodeData">(INP)�o�[�R�[�h�f�[�^</param>
    ''' <returns>�t�@�C���p�X��</returns>
    ''' <remarks>'V5.0.0.9�N</remarks>
    '''=========================================================================
    Public Function GetBarCodeFileName(ByVal strBarCodeData As String) As String    'V5.0.0.9�R
        'V5.0.0.9�R    Public Function GetBarCodeFileName(ByRef strBarCodeData As String) As String
        Dim ret As String
        Try
            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)�̑��݃`�F�b�N
            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
                ' "�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                Dim strMSG As String = MSG_15 & " File=" & strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                ret = String.Empty
            Else
                Select Case (BarCode_Data.Type)
                    Case BarcodeType.Taiyo
                        'V5.0.0.9�R                        ret = GetBarCodeFileNameTaiyo(strBarCodeData)
                        'V5.0.0.9�R      ��
                        If (BC_ReadCount < 1) Then
                            BC_ReadCount = (BC_ReadCount + 1)
                            BC_ReadDataFirst = strBarCodeData
                            ret = Nothing
                            Exit Try
                        End If
                        BC_ReadCount = 0
                        BC_ReadDataSecound = strBarCodeData
                        ret = GetBarCodeFileNameFromTwoData(BC_ReadDataFirst, BC_ReadDataSecound)
                        'V5.0.0.9�R      ��

                    Case BarcodeType.Walsin
                        ret = GetBarCodeFileNameWalsin(strBarCodeData)

                    Case BarcodeType.Standard
                        ret = GetBarCodeFileNameStandard(strBarCodeData)

                    Case Else
                        ret = String.Empty

                End Select
            End If

        Catch ex As Exception
            Dim strMSG As String = "BarCode_Data.GetBarCodeFileName() TRAP ERROR = " & ex.Message
            ret = String.Empty
        End Try

        Return ret

    End Function
#End Region

#Region "�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����(���z��)"
    '''=========================================================================
    ''' <summary>�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="strBarCodeData">(INP)�o�[�R�[�h�f�[�^</param>
    ''' <returns>�t�@�C���p�X��</returns>
    '''=========================================================================
    Private Function GetBarCodeFileNameTaiyo(ByVal strBarCodeData As String) As String      'V5.0.0.9�N
        'V5.0.0.9�N    Public Function GetBarCodeFileName(ByRef strBarCodeData As String) As String

        'Dim reader As System.IO.StreamReader
        Dim strLINE As String
        Dim strDAT(BC_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String

        Try
            'V5.0.0.9�N            '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
            'V5.0.0.9�N            ' �ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����
            'V5.0.0.9�N            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            'V5.0.0.9�N            If (BarcodeType.Walsin = BarCode_Data.Type) Then
            'V5.0.0.9�N                Return (GetBarCodeFileName2(BC_Data))
            'V5.0.0.9�N            End If
            'V5.0.0.9�N            '----- V4.11.0.0�A�� -----

            'V5.0.0.9�N            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)�̑��݃`�F�b�N
            'V5.0.0.9�N            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9�N                ' "�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
            'V5.0.0.9�N                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9�N                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9�N                strSetFullPath = ""
            'V5.0.0.9�N                Return (strSetFullPath)
            'V5.0.0.9�N            End If

            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)���I�[�v������
            'reader = New System.IO.StreamReader(strBCConvFileFullPath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))  ' �ڋq�쐬̧�قƂ̂���  V4.4.0.0-0
                ' �P�s�ÂÃ��[�h����
                While (reader.Peek() > 1)                                   ' TODO: 1 < ?
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","�ŕ������Ď��o��

                    ' �g���~���O�f�[�^����ݒ肷��
                    If (strDAT.Length >= 6) Then                            ' �g���~���O�f�[�^���܂ł̃f�[�^�͑��݂��邩 ?
                        ' �o�[�R�[�h�͓����� ?
                        If (strDAT(BC_IDX_BCOD) = strBarCodeData) And (strDAT(BC_IDX_DATA) <> "") Then
                            bFLG = True
                            ' �g���~���O�f�[�^���ݒ�
                            strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC_IDX_DATA) + ".tdc"

                            ' �g���~���O�f�[�^�̑��݃`�F�b�N
                            If (System.IO.File.Exists(strSetFullPath) = False) Then
                                '�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                                strMSG = MSG_15 + " File=" + strSetFullPath
                                Call Form1.Z_PRINT(strMSG)
                                strSetFullPath = ""
                                Exit While
                            End If

                            ' �o�[�R�[�h����ݒ肷��
                            gsBCInfo(0) = strDAT(BC_IDX_BCOD)               ' �o�[�R�[�h
                            gsBCInfo(1) = strDAT(BC_IDX_TYPE)               ' �i��
                            gsBCInfo(2) = strDAT(BC_IDX_OPTA)               ' �I�v�V�����`
                            gsBCInfo(3) = strDAT(BC_IDX_OPTB)               ' �I�v�V�����a
                            gsBCInfo(4) = strDAT(BC_IDX_RVAL)               ' ��R�l
                            Exit While
                        End If
                    End If

                End While

                '' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή��t�@�C��(�T�[�o)���N���[�Y����
                'reader.Close()
            End Using

            ' �o�[�R�[�h��������Ȃ������ꍇ
            If (bFLG = False) Then
                ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameTaiyo() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region
    'V5.0.0.9�R                          ��
#Region "2���ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����(���z��)"
    '''=========================================================================
    ''' <summary>�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="BC_ReadDataFirst">�o�[�R�[�h�P��ڂɓǍ��񂾃f�[�^</param>
    ''' <param name="BC_ReadDataSecound">�o�[�R�[�h�Q��ڂɓǍ��񂾃f�[�^</param>
    ''' <returns>�Y���g���~���O�f�[�^�t�@�C���p�X��</returns>
    '''=========================================================================
    Public Function GetBarCodeFileNameFromTwoData(ByVal BC_ReadDataFirst As String, ByVal BC_ReadDataSecound As String) As String

        'V5.0.0.9�R        Dim reader As System.IO.StreamReader
        Dim strLINE As String
        Dim strDAT(BC_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Integer = 0
        Dim strMSG As String

        Try
            'V5.0.0.9�R            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)�̑��݃`�F�b�N
            'V5.0.0.9�R            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9�R                ' "�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
            'V5.0.0.9�R                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9�R                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9�R                strSetFullPath = ""
            'V5.0.0.9�R                Return (strSetFullPath)
            'V5.0.0.9�R            End If

            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)���I�[�v������
            'V5.0.0.9�R            reader = New System.IO.StreamReader(strBCConvFileFullPath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))
                ' �P�s�ÂÃ��[�h����
                While (reader.Peek() > 1)
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","�ŕ������Ď��o��

                    ' �g���~���O�f�[�^����ݒ肷��
                    If (strDAT.Length >= 7) Then                            ' �g���~���O�f�[�^���܂ł̃f�[�^�͑��݂��邩 ?
                        ' �o�[�R�[�h�͓����� ?
                        If (strDAT(BC_IDX_BCOD) = BC_ReadDataFirst) Then
                            bFLG = 1
                            If (strDAT(BC_IDX_BCOD2) = BC_ReadDataSecound) AndAlso (strDAT(BC_IDX_DATA) <> "") Then
                                bFLG = 2
                                ' �g���~���O�f�[�^���ݒ�
                                strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC_IDX_DATA) + ".tdc"

                                ' �g���~���O�f�[�^�̑��݃`�F�b�N
                                If (System.IO.File.Exists(strSetFullPath) = False) Then
                                    '�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                                    strMSG = MSG_15 + " File=" + strSetFullPath
                                    Call Form1.Z_PRINT(strMSG)
                                    strSetFullPath = ""
                                    Exit While
                                End If

                                ' �o�[�R�[�h����ݒ肷��
                                gsBCInfo(0) = strDAT(BC_IDX_BCOD)               ' �o�[�R�[�h�P
                                gsBCInfo(1) = strDAT(BC_IDX_TYPE)               ' �i��
                                gsBCInfo(2) = strDAT(BC_IDX_OPTA)               ' �I�v�V�����`
                                gsBCInfo(3) = strDAT(BC_IDX_OPTB)               ' �I�v�V�����a
                                gsBCInfo(4) = strDAT(BC_IDX_RVAL)               ' ��R�l
                                gsBCInfo(5) = strDAT(BC_IDX_BCOD2)              ' �o�[�R�[�h�Q
                                Exit While
                            End If
                        End If
                    End If

                End While
            End Using

            'V5.0.0.9�R            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή��t�@�C��(�T�[�o)���N���[�Y����
            'V5.0.0.9�R           reader.Close()

            ' �o�[�R�[�h��������Ȃ������ꍇ
            If (bFLG < 2) Then
                If bFLG = 0 Then
                    ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " Code1=" + BC_ReadDataFirst
                ElseIf bFLG = 1 Then
                    ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " Code2=" + BC_ReadDataSecound
                Else
                    ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                    strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                End If
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameFromTwoData() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region
    'V5.0.0.9�R                          ��
    '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
#Region "�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����(Walsin)"
    '''=========================================================================
    ''' <summary>�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="strBarCodeData">(INP)�o�[�R�[�h�f�[�^</param>
    ''' <returns>�t�@�C���p�X��</returns>
    '''=========================================================================
    Private Function GetBarCodeFileNameWalsin(ByVal strBarCodeData As String) As String     'V5.0.0.9�N
        'V5.0.0.9�N    Public Function GetBarCodeFileName2(ByRef strBarCodeData As String) As String

        Dim strLINE As String
        Dim strDAT(BC2_IDX_MAX) As String
        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String
        Dim r As Integer

        Try
            'V5.0.0.9�N            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C��(�T�[�o)�̑��݃`�F�b�N
            'V5.0.0.9�N            If (System.IO.File.Exists(strBCConvFileFullPath) = False) Then
            'V5.0.0.9�N                ' "�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
            'V5.0.0.9�N                strMSG = MSG_15 + " File=" + strBCConvFileFullPath
            'V5.0.0.9�N                Call Form1.Z_PRINT(strMSG)
            'V5.0.0.9�N                strSetFullPath = ""
            'V5.0.0.9�N                Return (strSetFullPath)
            'V5.0.0.9�N            End If

            ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C�����I�[�v������
            Using reader As New StreamReader(strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))
                ' �P�s�ÂÃ��[�h����
                While (reader.Peek() > 1)                                   ' 1 < ?
                    strLINE = reader.ReadLine()
                    strDAT = strLINE.Split(",")                             ' ","�ŕ������Ď��o��

                    ' �g���~���O�f�[�^����ݒ肷��
                    If (strDAT.Length >= 2) Then                            ' �g���~���O�f�[�^���܂ł̃f�[�^�͑��݂��邩 ?
                        ' �o�[�R�[�h�͓����� ?
                        If (strDAT(BC2_IDX_BCOD) = strBarCodeData) And (strDAT(BC2_IDX_DATA) <> "") Then
                            bFLG = True
                            ' �g���~���O�f�[�^���ݒ�
                            strSetFullPath = gSysPrm.stDIR.gsTrimFilePath + "\" + strDAT(BC2_IDX_DATA) + ".tdcs"

                            ' �g���~���O�f�[�^�̑��݃`�F�b�N
                            If (System.IO.File.Exists(strSetFullPath) = False) Then
                                '�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                                strMSG = MSG_15 + " File=" + strSetFullPath
                                Call Form1.Z_PRINT(strMSG)
                                strSetFullPath = ""
                                Exit While
                            End If

                            ' �o�[�R�[�h����ݒ肷��
                            r = GetSizeCodeData(StSizeCode, strDAT(BC2_IDX_BCOD))
                            If (r >= cFRS_NORMAL) Then
                                gsBCInfo(0) = strDAT(BC2_IDX_BCOD)      ' �o�[�R�[�h
                                gsBCInfo(1) = StSizeCode(r).SizePrt     ' ��T�C�Y
                            Else
                                gsBCInfo(0) = strDAT(BC2_IDX_BCOD)      ' �o�[�R�[�h
                                gsBCInfo(1) = "??"                      ' ��T�C�Y
                            End If
                            Exit While
                        End If
                    End If

                End While

            End Using

            ' �o�[�R�[�h��������Ȃ������ꍇ
            If (bFLG = False) Then
                ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                strMSG = MSG_157 + " File=" + strBCConvFileFullPath
                Call Form1.Z_PRINT(strMSG)
                strSetFullPath = ""
            End If

            Return (strSetFullPath)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameWalsin() TRAP ERROR = " + ex.Message
            Return ("")
        End Try
    End Function
#End Region

#Region "SizeCode.ini���\���̂Ɏ�荞��"
    '''=========================================================================
    ''' <summary>SizeCode.ini���\���̂Ɏ�荞��</summary>
    ''' <param name="StSizeCode">(I/O)��T�C�Y�p�\����</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function IniSizeCodeData(ByRef StSizeCode() As SizeCode_Data_Info) As Integer

        Dim Idx As Integer = 0
        Dim strLINE As String
        Dim strDAT(MAX_SizeCode) As String
        Dim strMSG As String

        Try
            ' SizeCode.ini�̑��݃`�F�b�N
            If (System.IO.File.Exists(PATH_SIZCDE_DATA) = False) Then
                ' "�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                strMSG = MSG_15 + " File=" + PATH_SIZCDE_DATA
                Call Form1.Z_PRINT(strMSG)
                Return (cFRS_FIOERR_INP)
            End If

            ' SizeCode.ini���I�[�v������
            Using reader As New StreamReader(PATH_SIZCDE_DATA, Encoding.GetEncoding("Shift_JIS"))
                ' �P�s�ÂÃ��[�h����
                While (reader.Peek() > 1)                               ' 1 < ?
                    strLINE = reader.ReadLine()
                    ' ","�ŕ������Ď��o��
                    strDAT = strLINE.Split(",")

                    ' SizeCode.ini���\���̂�ݒ肷��
                    If (strDAT.Length >= 2) And (Idx < StSizeCode.Length) Then
                        ' ��T�C�Y�p�\���̂�ݒ肷��
                        StSizeCode(Idx).SizeBar = strDAT(0)             ' ��T�C�Y(�o�[�R�[�h��3,4������)
                        StSizeCode(Idx).SizePrt = strDAT(1)             ' ��T�C�Y(�\���p)
                        Idx = Idx + 1
                    End If
                End While
            End Using

            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.IniSizeCodeData() TRAP ERROR = " + ex.Message
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�o�[�R�[�h������T�C�Y�p�\���̂���擾����"
    '''=========================================================================
    ''' <summary>�o�[�R�[�h����StSizeCode����擾����</summary>
    ''' <param name="StSizeCode">(INP)��T�C�Y�p�\����</param>
    ''' <param name="strBar">    (INP)�o�[�R�[�h</param>
    ''' <returns>0 >= StSizeCode�̃C���f�b�N�X, -1=�G���[</returns> 
    '''=========================================================================
    Public Function GetSizeCodeData(ByRef StSizeCode() As SizeCode_Data_Info, ByRef strBar As String) As Integer

        Dim Idx As Integer = 0
        Dim strMSG As String

        Try
            ' �o�[�R�[�h������T�C�Y�p�\���̂���擾����
            strMSG = strBar.Substring(2, 2)                             ' �o�[�R�[�h��3,4������
            For Idx = 0 To StSizeCode.Length
                If (StSizeCode(Idx).SizeBar = strMSG) Then
                    Return (Idx)
                End If
            Next Idx

            Return (-1)

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.GetSizeCodeData() TRAP ERROR = " + ex.Message
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '----- V4.11.0.0�A�� -----
#Region "�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����(�W���I�v�V����)"
    '''=========================================================================
    ''' <summary>�ް���ގ�M�ް�����w��̕������ҏW��̧���߽���쐬����</summary>
    ''' <param name="strBarCodeData">(INP)�o�[�R�[�h�f�[�^</param>
    ''' <returns>�t�@�C���p�X��</returns>
    ''' <remarks>'V5.0.0.9�N</remarks>
    '''=========================================================================
    Private Function GetBarCodeFileNameStandard(ByVal strBarCodeData As String) As String

        Dim strSetFullPath As String = ""
        Dim bFLG As Boolean = False
        Dim strMSG As String

        Try
            ' �o�[�R�[�h�f�[�^���猟���L�[�ƂȂ镔����������擾����
            Dim key As String = GetSubstringKey(strBarCodeData)
            If (String.Empty <> key) Then                               'V5.0.0.9�S

                ' �o�[�R�[�h�ƃg���~���O�f�[�^�̑Ή�CSV�t�@�C����ǂݍ���
                Dim lines() As String = System.IO.File.ReadAllLines(
                    strBCConvFileFullPath, Encoding.GetEncoding("Shift_JIS"))

                For Each line As String In lines
                    ' �s�̐擪������������L�[�Ɣ�r����
                    If (line.StartsWith(key)) Then
                        ' �擪�����񂪃L�[�ƈ�v����
                        bFLG = True

                        ' �g���~���O�f�[�^���ݒ�
                        strSetFullPath = gSysPrm.stDIR.gsTrimFilePath &
                            "\" & line.Replace(key, String.Empty) & File.Extension

                        ' ������,���폜
                        key = key.Remove(key.Length - 1)                'V5.0.0.9�S

                        ' �g���~���O�f�[�^�̑��݃`�F�b�N
                        If (System.IO.File.Exists(strSetFullPath) = False) Then
                            '�w�肳�ꂽ�t�@�C���͑��݂��܂��� File=xxxxxxxxxxxx"
                            strMSG = MSG_15 & " Key=[ " & key & " ]" & vbCrLf &
                                "  File=" & strSetFullPath

                            Call Form1.Z_PRINT(strMSG)
                            strSetFullPath = ""
                        Else
                        gsBCInfo(0) = strBarCodeData                    'V5.0.0.9�S
                        gsBCInfo(1) = key                               'V5.0.0.9�S
                        End If

                        Exit For
                    End If
                Next line

                ' �o�[�R�[�h��������Ȃ������ꍇ
                If (bFLG = False) Then
                    ' ������,���폜
                    key = key.Remove(key.Length - 1)                    'V5.0.0.9�S

                    'V5.0.0.9�S                    ' "�w�肳�ꂽ�o�[�R�[�h�͑��݂��܂��� File=xxxxxxxxxxxx"
                    ' �w�肳�ꂽ�L�[��������܂���                           'V5.0.0.9�S
                    strMSG = BarCode_Data_009 & vbCrLf &
                        "  File=" & strBCConvFileFullPath & "  Key=[ " & key & " ]"

                    Call Form1.Z_PRINT(strMSG)
                    strSetFullPath = ""
                End If

            Else                                                        'V5.0.0.9�S
                ' �擾������̎w�肪�o�[�R�[�h�������𒴉߂���
                Dim tmp As New List(Of String)
                For Each s As Tuple(Of Integer, Integer) In SubStr
                    tmp.Add(String.Join("-", s.Item1 + 1, s.Item2))
                Next
                ' �o�[�R�[�h�������𒴉߂��܂� (#-#, #-#)
                strMSG = String.Format(BarCode_Data_010, String.Join(", ", tmp))
                Form1.Z_PRINT(strMSG)
                strSetFullPath = String.Empty
            End If

            ' �g���b�v�G���[������
        Catch ex As Exception
            strMSG = "BarCode_Data.GetBarCodeFileNameStandard() TRAP ERROR = " & ex.Message
            strSetFullPath = String.Empty
        End Try

        Return (strSetFullPath)

    End Function
#End Region

#Region "�o�[�R�[�h�f�[�^���畔������������o��"
    ''' <summary>�o�[�R�[�h�f�[�^���畔������������o��</summary>
    ''' <param name="barcodeData">��M�o�[�R�[�h�f�[�^</param>
    ''' <returns>csv�t�@�C�����ŊY���s�����L�[�ƂȂ镶����,String.Empty=�擾BARCODE_SUBSTR1�܂���2�̎w�肪�������𒴉߂���</returns>
    ''' <remarks>'V5.0.0.9�N</remarks>
    Private Function GetSubstringKey(ByVal barcodeData As String) As String
        Dim ret As String = barcodeData & ","

        Try
            If (0 < SubStr.Count) Then
                ' ����������擾�w�肠��
                Dim tmp As New List(Of String)()

                For Each subStr As Tuple(Of Integer, Integer) In BarCode_Data.SubStr
                    If ((subStr.Item1 + subStr.Item2) <= barcodeData.Length) Then
                        ' �o�[�R�[�h�f�[�^�������𒴉߂��Ȃ���Ε���������擾
                        tmp.Add(barcodeData.Substring(subStr.Item1, subStr.Item2))
                    Else
                        ' �擾�s��
                        tmp.Add(String.Empty)   'V5.0.0.9�S �ǉ�
                    End If
                Next

                If (False = tmp.Contains(String.Empty)) Then
                    ret = String.Join(",", tmp) & ","
                Else
                    ret = String.Empty          'V5.0.0.9�S �ǉ�
                End If
            End If

        Catch ex As Exception
            ret = barcodeData & ","
        End Try

        Return ret

    End Function
#End Region

#Region "�o�[�R�[�h�f�[�^�Ŏw��̃t�@�C�������[�h����"
    '''=========================================================================
    ''' <summary>�o�[�R�[�h�f�[�^�Ŏw��̃t�@�C�������[�h����</summary>
    ''' <param name="pPath">(INP)�t�@�C���p�X��</param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns> 
    '''=========================================================================
    Public Function DataLoadBC(ByRef pPath As String) As Integer

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
                Return (r)
            End If

            ' �I������
            Call Form1.ClearCounter(1)                                  ' ���Y�Ǘ��f�[�^�N���A
            Call ClrTrimPrnData()                                       ' �g���~���O���ʈ�����ڏ����� 
            Call BC_Info_Disp(1)                                        ' �o�[�R�[�h����\������
            '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
            '�ڕW�l�A��������̕\��(�V���v���g���}�̏ꍇ)
            If (gMachineType = MACHINE_TYPE_436S) Then
                SimpleTrimmer.SetTarget(typResistorInfoArray(1).dblTrimTargetVal, typResistorInfoArray(1).dblInitTest_LowLimit, typResistorInfoArray(1).dblInitTest_HighLimit, typResistorInfoArray(1).dblFinalTest_LowLimit, typResistorInfoArray(1).dblFinalTest_HighLimit, typResistorInfoArray(1).dblTrimTargetVal_Save)
            End If
            '----- V4.11.0.0�A�� -----

            'V5.0.0.9�R Call SetMousePointer(Form1, False)                          ' �����v����(mouse pointer)
            Return (cFRS_NORMAL)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BarCode_Data.DataLoadBC() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)

        Finally                                                         ' V5.0.0.9�R
            SetMousePointer(Form1, False)                               ' �����v����(mouse pointer)
        End Try

    End Function
#End Region

#Region "�o�[�R�[�h����\������"
    '''=========================================================================
    ''' <summary>�o�[�R�[�h����\������</summary>
    ''' <param name="Md">(INP)���[�h(0=������, 1=�\��)</param>
    '''=========================================================================
    Public Sub BC_Info_Disp(ByVal Md As Integer)

        Dim strMSG As String

        Try
            ' ���z�Гa����/(WALSIN�aSL436S�Ή�)�łȂ����NOP
            'If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) Then Return
            'V5.0.0.9�N            If (gSysPrm.stCTM.giSPECIAL <> customTAIYO) And (gSysPrm.stCTM.giSPECIAL <> customWALSIN) Then Return ' V4.11.0.0�A
            'V5.0.0.9�S            If (BarcodeType.Taiyo <> BarCode_Data.Type) AndAlso (BarcodeType.Walsin <> BarCode_Data.Type) Then Return 'V5.0.0.9�N
            If (BarcodeType.None = BarCode_Data.Type) Then Return 'V5.0.0.9�S
            ' ������
            If (Md = 0) Then
                gsBCInfo(0) = ""
                gsBCInfo(1) = ""
                gsBCInfo(2) = ""
                gsBCInfo(3) = ""
                gsBCInfo(4) = ""
                gsBCInfo(5) = ""                    'V5.0.0.9�R
            End If

            ' �o�[�R�[�h����\������
            'If (gSysPrm.stTMN.giMsgTyp = 0) Then
            '    Form1.lblQRData.Text = "[�o�[�R�[�h���]" & vbCrLf & _
            '                            " �o�[�R�[�h  �F" & gsBCInfo(0) & vbCrLf & _
            '                            " �i��        �F" & gsBCInfo(1) & vbCrLf & _
            '                            " �I�v�V�����`�F" & gsBCInfo(2) & vbCrLf & _
            '                            " �I�v�V�����a�F" & gsBCInfo(3) & vbCrLf & _
            '                            " ��R�l      �F" & gsBCInfo(4) & vbCrLf
            'Else
            '    Form1.lblQRData.Text = "[BAR CODE INFORMATION]" & vbCrLf & _
            '                             " Bar Code   :" & gsBCInfo(0) & vbCrLf & _
            '                             " Type       :" & gsBCInfo(1) & vbCrLf & _
            '                             " Option A   :" & gsBCInfo(2) & vbCrLf & _
            '                             " Option B   :" & gsBCInfo(3) & vbCrLf & _
            '                             " Res Value  :" & gsBCInfo(4) & vbCrLf
            'End If
#If False Then                          'V5.0.0.9�N
            '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
            If (gSysPrm.stCTM.giSPECIAL = customTAIYO) Then
                Form1.GrpQrCode.Text = BarCode_Data_001 ' "[�o�[�R�[�h���]"
                Form1.lblQRData.Text = gsBCInfo(0) & vbCrLf & _
                                        BarCode_Data_002 & gsBCInfo(0) & vbCrLf & _
                                        BarCode_Data_003 & gsBCInfo(1) & vbCrLf & _
                                        BarCode_Data_004 & gsBCInfo(2) & vbCrLf & _
                                        BarCode_Data_005 & gsBCInfo(3) & vbCrLf & _
                                        BarCode_Data_006 & gsBCInfo(4) & vbCrLf
            Else
                ' "[�o�[�R�[�h���]" "�o�[�R�[�h" "��T�C�Y"
                Form1.GrpQrCode.Text = BarCode_Data_001 ' "[�o�[�R�[�h���]"
                Form1.lblQRData.Text = " Bar Code  :" & gsBCInfo(0) & vbCrLf & _
                                       " Size code :" & gsBCInfo(1) & vbCrLf
            End If
            '----- V4.11.0.0�A�� -----
#Else                                   'V5.0.0.9�N
            Select Case (BarCode_Data.Type)
                Case BarcodeType.Taiyo
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[�o�[�R�[�h���]"
                    'V5.0.0.9�R  BarCode_Data_007, BarCode_Data_008�ɕύX
                    Form1.lblQRData.Text = BarCode_Data_007 & gsBCInfo(0) & vbCrLf & _
                                           BarCode_Data_008 & gsBCInfo(5) & vbCrLf & _
                                           BarCode_Data_003 & gsBCInfo(1) & vbCrLf & _
                                           BarCode_Data_004 & gsBCInfo(2) & vbCrLf & _
                                           BarCode_Data_005 & gsBCInfo(3) & vbCrLf & _
                                           BarCode_Data_006 & gsBCInfo(4) & vbCrLf

                Case BarcodeType.Walsin
                    ' "[�o�[�R�[�h���]" "�o�[�R�[�h" "��T�C�Y"
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[�o�[�R�[�h���]"
                    Form1.lblQRData.Text = " Bar Code  :" & gsBCInfo(0) & vbCrLf & _
                                           " Size code :" & gsBCInfo(1) & vbCrLf

                Case BarcodeType.Standard           'V5.0.0.9�S
                    Form1.GrpQrCode.Text = BarCode_Data_001 ' "[�o�[�R�[�h���]"
                    Form1.lblQRData.Text = BarCode_Data_012 & gsBCInfo(0) & vbCrLf &
                                           BarCode_Data_011 & gsBCInfo(1) & vbCrLf

                Case Else
                    ' DO NOTHING
            End Select
#End If
            Form1.lblQRData.Refresh()

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "BarCode_Data.BC_Info_Disp() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region
End Module
