'===============================================================================
'   Description  : �������(���[���a����) V1.18.0.0�B
'
'   Copyright(C) : OMRON LASERFRONT INC. 2014
'
'===============================================================================
Imports System.IO                       'V4.4.0.0-0
Imports System.Text                     'V4.4.0.0-0
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module QR_Print
#Region "�y�萔/�ϐ��̒�`�z"
    '===========================================================================
    '   �萔/�ϐ��̒�`
    '===========================================================================
    '----- �N���X�I�u�W�F�N�g -----
    Private ObjPrint As DllPrintString.PrintString = Nothing            ' ����p�I�u�W�F�N�g(DllPrintString.dll)
    'Private strFont As String = "�l�r �o�S�V�b�N"                     ' ����p�t�H���g 
    'Private strFont As String = "MS Gothic"                             ' ����p�t�H���g 
    Private strFont As String = QR_Print_001                             ' ����p�t�H���g 
    Private iFontSz As Integer = 10                                     ' �t�H���g�T�C�Y

    '----- ����ʒu -----
    Private Const KEISEN_MAX As Integer = 5                             ' �r����("---------------")
    Private Const ITEM_MAX As Integer = 49 + KEISEN_MAX                 ' ������ڐ�
    Private Const LINE_SZ As Integer = 24                               ' 1�s�̕�����
    Private Const HED_SZ As Integer = 13                                ' ���o��������
    Private Const HED_S2 As Integer = 11                                ' ���o��������2
    Private Const HED_S3 As Integer = 7                                 ' ���o��������3(Lot No.�p)
    Private Const PRT_SZ As Integer = 11                                ' ���������
    Private Const PRT_S2 As Integer = 6                                 ' ���������2
    Private Const PRT_S3 As Integer = 7                                 ' ���������3
    Private Const PRT_LOT As Integer = 17                               ' ���������4(Lot No.�p)
    Private Const HEAD_POSX As Integer = 1                              ' ���o��������WX
    Private Const HEAD_POSY As Integer = 5                              ' ���o��������WY

    '---------------------------------------------------------------------------
    '   �g���~���O���ʈ���p�\����(���[���a�p) 
    '---------------------------------------------------------------------------
    Public Structure TRIM_RESULT_PRINT_ROHM_INFO
        Dim bAutoMode As Boolean                                        ' ����/�蓮     (False=�蓮�^�], True=�����^�])
        Dim bFlgPrint As Boolean                                        ' ����ς݃t���O(False=�����  , True=�����)

        Dim MachineNo As String                                         '  1.���u�ԍ�(10�� "SL436Rxxxx") 
        '                                                               '  2.�󔒍s
        '                                                               '  3.Lot Result 
        Dim Start_day As String                                         '  4.�J�n��(2014/Feb/20)
        Dim Start_time As String                                        '  5.�J�n����(hh:mm:ss)
        Dim End_day As String                                           '  6.�I����(2014/Feb/20)
        Dim End_time As String                                          '  7.�I������(hh:mm:ss)
        Dim Prod_time As String                                         '  8.�J�n�`�I���܂łɗv��������(hh:mm:ss)
        '                                                               '  9.�󔒍s
        Dim Lot_No As String                                            ' 10.QR�R�[�h���̃��b�g�ԍ�
        Dim Prot_Typ As String                                          ' 11.�i��(QR�R�[�h�̃^�C�v��ݒ肷��)
        Dim IEC_Tol As String                                           ' 12.�ڕW��R�l(999.999)
        Dim Cal_RV As String                                            ' 13.�ڕW��R�l(�J�b�g�I�t���l�������ڕW�l)
        Dim LaserPower As String                                        ' 14.���H�ʏo��(9.99W)
        Dim Stop1 As String                                             ' 15.L�^�[���|�C���g(%)
        Dim Stop2 As String                                             ' 16.�g���~���O�����|�C���g(%) (�J�b�g�I�t(%))
        Dim Speed1 As String                                            ' 17.���H���x1(L�^�[���O)(9.9mm/s)
        Dim Speed2 As String                                            ' 18.���H���x2(L�^�[����)(9.9mm/s)
        Dim Qrate1 As String                                            ' 19.Q���[�g1(L�^�[���O) (9.9kHz)
        Dim Qrate2 As String                                            ' 20.Q���[�g2(L�^�[����) (9.9kHz)
        Dim GlvPosX As String                                           ' 21.�K���o�m�X�^�[�g�|�C���g(�J�b�g�ʒuXY)
        Dim GlvPosY As String                                           ' 
        Dim CompOffset As String                                        ' 22.�J�b�g�I�t(�����e�s���̂��ߖ��Ή����J�b�g�I�t(%)��ݒ�)
        '                                                               ' 23.�󔒍s
        Dim sTrimRate As String                                         ' 24.���ϐ؏㗦(���ϒ�R�l/�ڕW��R�l)
        Dim TrimRate As Double                                          '    ���ϐ؏㗦(���ϒ�R�l/�ڕW��R�l)
        Dim MTBF As String                                              ' 25.�G���[�����Ԋu(���ό̏�Ԋu)
        Dim MTTR As String                                              ' 26.�G���[������̕�������(���ϕ�������)
        Dim sTol_Sheet As String                                        ' 27.�g�[�^������(���u�ɓ������ꂽ����)
        Dim Tol_Sheet As Integer                                        '    �g�[�^������(���u�ɓ������ꂽ����) ��m_lPlateCount(�v���[�g������)��ݒ�
        Dim NG_Cancel As Integer                                        ' 28.NG�L�����Z����(���ݒ肳���P�[�X���Ȃ����ߖ��Ή�)
        Dim sJudgeValue As String                                       ' 29.�s�������(�SNG��/�g���~���O��)
        Dim JudgeValue As Double                                        '    �s�������(�SNG��/�g���~���O��)
        Dim OffsetX As String                                           ' 30.�I�t�Z�b�g(BP�I�t�Z�b�gXY)
        Dim OffsetY As String                                           ' 
        Dim sProd_Count As String                                       ' 31.�g���~���O�`�b�v��
        Dim Prod_Count As Integer                                       '    �g���~���O�`�b�v��(m_lGoodCount(�Ǖi��R��) + m_lNgCount(�s�ǒ�R��))
        Dim sTol_NG_Sheet As String                                     ' 32.��NG�����
        Dim Tol_NG_Sheet As Integer                                     '    ��NG�����
        '                                                               ' 33.�󔒍s
        '                                                               ' 34.PCS (%)
        Dim sPretest_Hi_Fail As String                                  ' 35.�����l����s�ǂ����ߐ�
        Dim Pretest_Hi_Fail As Integer                                  '    �����l����s�ǂ����ߐ�   (m_lITHINGCount(IT HI NG��)��ݒ�)
        Dim sPretest_Lo_Fail As String                                  ' 36.�����l�����s�ǂ����ߐ�
        Dim Pretest_Lo_Fail As Integer                                  '    �����l�����s�ǂ����ߐ�   (m_lITLONGCount(IIT LO NG��)��ݒ�)
        Dim sPretest_Open As String                                     ' 37.�����l����ݕs�ǂ����ߐ�
        Dim Pretest_Open As Integer                                     '    �����l����ݕs�ǂ����ߐ�   (m_lITOVERCount(IT���ް�ݼސ�)��ݒ�)
        Dim sFinal_test_Hi_Fail As String                               ' 38.���ݸތ�̏���s�ǂ����ߐ�
        Dim Final_test_Hi_Fail As Integer                               '    ���ݸތ�̏���s�ǂ����ߐ�(m_lFTHINGCount(FT HI NG��)��ݒ�)
        Dim sFinal_test_Lo_Fail As String                               ' 39.���ݸތ�̉����s�ǂ����ߐ�
        Dim Final_test_Lo_Fail As Integer                               '    ���ݸތ�̉����s�ǂ����ߐ�(m_lFTLONGCount(FT LO NG��)��ݒ�)
        '                                                               ' 40.�󔒍s
        Dim sTrim_OK As String                                          ' 41.�Ǖi���ߐ� + %(�g���~���O�`�b�v���Ƃ̔䗦)
        Dim Trim_OK As Integer                                          '    �Ǖi���ߐ� (m_lITHINGCount(IT HI NG��)��ݒ�)
        Dim sInit_OK As String                                          ' 42.�C�j�V�����Ǖi���ߐ� + %(�g���~���O�`�b�v���Ƃ̔䗦)
        Dim Init_OK As Integer                                          '    �C�j�V�����Ǖi���ߐ�(�g���~���O�`�b�v�� - (IT NG���ߐ� + FT NG���ߐ�))
        Dim Trim_NG As Integer                                          '    �s�Ǖi���ߐ�(���[�N)
        Dim sTotal_OK As String                                         ' 43.���Ǖi���ߐ�(�Ǖi���ߐ��Ɠ����l)
        Dim Total_OK As Integer                                         '    ���Ǖi���ߐ�(�Ǖi���ߐ��Ɠ����l)
        '
        Dim sInt_RV_NG As String                                        ' 44.�����l����s�ǂ����ߐ� + �����l�����s�ǂ����ߐ�
        Dim Int_RV_NG As Integer                                        '    �����l����s�ǂ����ߐ� + �����l�����s�ǂ����ߐ�
        Dim sAT_Trim_NG As String                                       ' 45.�SNG�`�b�v��
        Dim AT_Trim_NG As Integer                                       '    �SNG�`�b�v��
        '                                                               ' 46.�󔒍s
        Dim sAlmCnt As String                                           ' 47.�A���[��������
        Dim AlmCnt As Integer                                           '    �A���[��������
        '                                                               ' 48.�󔒍s
        '                                                               ' 49.�A���[����������/�G���[���e
        Dim AlarmST_time As String                                      '    �A���[����������("hh:mm:ss")

        '----- ���[�N�恫 -----
        Dim STtime As Double                                            ' �J�n����(double)
        Dim EDtime As Double                                            ' �I������(double)
        Dim AlmSTtime As Double                                         ' �װђ�~�J�n����(double)
        Dim AlmEDtime As Double                                         ' �װђ�~�I������(double)
        Dim AlmTotaltime As Double                                      ' �װђ�~İ�َ���(double)

        Dim Trim_TotalVal As Double                                     ' ���ݸނ��ꂽ���߂̒�R�l(���v)
        Dim Trim_TotalValCnt As Double                                  ' ���ݸނ��ꂽ���߂̒�R�l(�v�Z�p���v)
        Dim Trim_TotalValKT As Short                                    ' ���ݸނ��ꂽ���߂̒�R�l(��)
        Dim Pdt_Sheet As Integer                                        ' ���ݸ޽ð�ނŏ������������
        Dim Edg_Fail As Integer                                         ' ۯĒ��̔F���s�Ǌ����
        '----- ���[�N�恪 -----
    End Structure

    Public stPRT_ROHM As TRIM_RESULT_PRINT_ROHM_INFO                    ' �g���~���O���ʈ���p�\����(���[���a�p)
    Public gbAlarmFlg As Boolean = False                                ' �A���[�������t���O(False=�A���[����~, True =�A���[������)
    Public gFPATH_QR_ALARM As String = "C:\TRIMDATA\LOG\qr_alarm.txt"   ' �A���[���t�@�C����

#End Region

#Region "�y���\�b�h��`�z"
    '---------------------------------------------------------------------------
    '   ����pDLL�Ăяo��
    '---------------------------------------------------------------------------
#Region "�����������"
    '''=========================================================================
    ''' <summary>�����������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Print_Init() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���[���a�d�l�łȂ����NOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If

            ' ��������ݒ菈��
            ObjPrint = New DllPrintString.PrintString                   ' ����p�I�u�W�F�N�g����
            r = ObjPrint.Print_Init(strFont, iFontSz)                   ' ����������
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Print_Init() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "����I������"
    '''=========================================================================
    ''' <summary>����I������</summary>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Print_End() As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���[���a�d�l�łȂ����NOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If

            ' ����I������
            If (ObjPrint Is Nothing) Then                               ' ����p�I�u�W�F�N�g�������Ȃ�NOP
                Return (cFRS_NORMAL)
            End If
            r = ObjPrint.Print_End()                                    ' ����p�I�u�W�F�N�g�J��  
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Print_End() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region

#Region "�������"
    '''=========================================================================
    ''' <summary>�������</summary>
    ''' <param name="strDAT">(INP)���������</param>
    ''' <param name="strHEAD"></param>
    ''' <param name="X"></param>
    ''' <param name="Y"></param>
    ''' <returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function Print_Data(ByVal strDAT As String, ByVal strHEAD As String, ByVal X As Integer, ByVal Y As Integer) As Integer

        Dim r As Integer
        Dim strMSG As String

        Try
            ' ���[���a�d�l�łȂ����NOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
                Return (cFRS_NORMAL)
            End If
            ' ����p�I�u�W�F�N�g�������Ȃ�NOP
            If (ObjPrint Is Nothing) Then
                Return (cFRS_NORMAL)
            End If

            ' �������
            r = ObjPrint.Print_Data(strDAT, strHEAD, X, Y)
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Print_Data() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)
        End Try
    End Function
#End Region
    '---------------------------------------------------------------------------
    '   ����p���ڏW�v����
    '---------------------------------------------------------------------------
#Region "�g���~���O���ʈ�����ڏ�����"
    '''=========================================================================
    ''' <summary>�g���~���O���ʈ�����ڏ�����</summary>
    ''' <remarks>�������͉��L����Call�����
    ''' �@�@�@ �EQR�f�[�^��M��
    ''' �@�@�@ �ECLR�{�^��������(IDLE��Ԃ̃��C�����, �ꎞ��~���)
    ''' �@�@�@ �E�f�[�^���[�h�R�}���h��(�蓮)
    ''' </remarks>
    '''=========================================================================
    Public Sub ClrTrimPrnData()

        Dim strMSG As String

        Try
            ' ������ڏ�����
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            If (giAppMode = APP_MODE_TRIM) Then Return

            gbFgContinue = False                                        ' �����^�]�p���t���OOFF   

            With stPRT_ROHM
                .bAutoMode = False                                      ' False = �蓮�^�]
                .bFlgPrint = False                                      ' False = �����

                .MachineNo = ""                                         '  1.���u�ԍ�
                '                                                       '  2.�󔒍s
                '                                                       '  3.Lot Result
                .Start_day = ""                                         '  4.�J�n��
                .Start_time = ""                                        '  5.�J�n����
                .End_day = ""                                           '  6.�I����
                .End_time = ""                                          '  7.�I������
                .Prod_time = ""                                         '  8.�J�n�`�I���܂łɗv��������
                '                                                       '  9.�󔒍s
                .Lot_No = ""                                            ' 10.QR�R�[�h���̃��b�g�ԍ�
                .Prot_Typ = ""                                          ' 11.�i��(QR�R�[�h�̃^�C�v��ݒ肷��)
                .IEC_Tol = ""                                           ' 12.�ڕW��R�l
                .Cal_RV = ""                                            ' 13.�ڕW��R�l(�J�b�g�I�t���l�������ڕW�l)
                .LaserPower = ""                                        ' 14.���H�ʏo��
                .Stop1 = ""                                             ' 15.L�^�[���|�C���g(%)
                .Stop2 = ""                                             ' 16.�J�b�g�I�t(%)
                .Speed1 = ""                                            ' 17.���H���x1(L�^�[���O)
                .Speed2 = ""                                            ' 18.���H���x2(L�^�[����)
                .Qrate1 = ""                                            ' 19.Q���[�g1(L�^�[���O)
                .Qrate2 = ""                                            ' 20.Q���[�g2(L�^�[����)
                .GlvPosX = ""                                           ' 21.�K���o�m�X�^�[�g�|�C���g(�J�b�g�ʒuXY)
                .GlvPosY = ""
                '                                                       ' 22.�J�b�g�I�t(�����e�s���̂��ߖ��Ή�)
                '                                                       ' 23.�󔒍s
                .TrimRate = 0.0#                                        ' 24.���ϐ؏㗦
                .MTBF = ""                                              ' 25.�G���[�����Ԋu(���ό̏�Ԋu)
                .MTTR = ""                                              ' 26.�G���[������̕�������(���ϕ�������)
                .Tol_Sheet = 0                                          ' 27.�g�[�^������(���u�ɓ������ꂽ����)
                '                                                       ' 28.NG�L�����Z����(���ݒ肳���P�[�X���Ȃ����ߖ��Ή�)
                .JudgeValue = 0.0#                                      ' 29.�s�������(�SNG��/�g���~���O��)
                .OffsetX = ""                                           ' 30.�I�t�Z�b�g(BP�I�t�Z�b�gXY)
                .OffsetY = ""
                .Prod_Count = 0                                         ' 31.�g���~���O�`�b�v��
                .Tol_NG_Sheet = 0                                       ' 32.��NG�����
                '                                                       ' 33.�󔒍s
                '                                                       ' 34.PCS (%)
                .Pretest_Hi_Fail = 0                                    ' 35.�����l����s�ǂ����ߐ�
                .Pretest_Lo_Fail = 0                                    ' 36.�����l�����s�ǂ����ߐ�
                .Pretest_Open = 0                                       ' 37.�����l����ݕs�ǂ����ߐ�
                .Final_test_Hi_Fail = 0                                 ' 38.���ݸތ�̏���s�ǂ����ߐ�
                .Final_test_Lo_Fail = 0                                 ' 39.���ݸތ�̉����s�ǂ����ߐ�
                '                                                       ' 40.�󔒍s
                .Trim_OK = 0                                            ' 41.�Ǖi���ߐ�
                .Init_OK = 0                                            ' 42.�C�j�V�����Ǖi���ߐ�
                .Total_OK = 0                                           ' 43.���Ǖi���ߐ�
                .Int_RV_NG = 0                                          ' 44.�C�j�V����NG�`�b�v�� + �t�@�C�i��NG�`�b�v��
                .AT_Trim_NG = 0                                         ' 45.�SNG�`�b�v��
                '                                                       ' 46.�󔒍s
                .AlmCnt = 0                                             ' 47.�A���[��������
                '                                                       ' 48.�󔒍s
                '                                                       ' 49.�A���[����������/�G���[���e
                .AlarmST_time = String.Empty                            '    �A���[����������("hh:mm:ss")

                ' ���[�N�揉����
                .STtime = 0.0#                                          ' �J�n����(double)
                .EDtime = 0.0#                                          ' �I������(double)
                .AlmSTtime = 0.0#                                       ' �װђ�~�J�n����(double)
                .AlmEDtime = 0.0#                                       ' �װђ�~�I������(double)
                .AlmTotaltime = 0.0#                                    ' �װђ�~İ�َ���(double)

                .Trim_TotalVal = 0.0#                                   ' ���ݸނ��ꂽ���߂̒�R�l(���v)
                .Trim_TotalValCnt = 0.0#                                ' ���ݸނ��ꂽ���߂̒�R�l(�v�Z�p���v)
                .Trim_TotalValKT = 0                                    ' ���ݸނ��ꂽ���߂̒�R�l(��)
            End With

            ' ����p�e�L�X�g�{�b�N�X(TxtBoxPrint(��\��))�N���A
            Form1.TxtBoxPrint.Text = ""

            ' ����p�A���[���t�@�C�����폜����
            Call DeleteAlarmData(gFPATH_QR_ALARM)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.ClrTrimPrnData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O���ʈ�����ڂ̃f�[�^���Z�b�g����(����O�ɐݒ�\�Ȃ���)"
    '''=========================================================================
    ''' <summary>�g���~���O���ʈ�����ڂ̃f�[�^���Z�b�g����</summary>
    ''' <remarks>����O�ɐݒ�\�Ȃ��̂̂݁A�����ŃZ�b�g����</remarks>
    '''=========================================================================
    Public Sub SetTrimPrnData()

        Dim TotalTime As Double = 0                                     ' ���v����(sec)
        Dim UpTime As Double = 0                                        ' 24���ԍl��(sec)
        Dim OpeTime As Double = 0                                       ' �ғ�����(sec)
        Dim KosyoIntv As Double = 0                                     ' ���ό̏�Ԋu(sec)
        Dim RecovTime As Double = 0                                     ' ���ϕ�������(sec)
        Dim iWK As Integer = 0
        Dim dblWK As Double

        Dim strMSG As String

        Try
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            With stPRT_ROHM
                '  1.MachineNo ���u�ԍ�
                strMSG = LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S("TMENU", "DEVNUM", "C:\TRIM\tky.ini", "SL436R")
                .MachineNo = strMSG.PadLeft(HED_S2)
                '  2.�󔒍s
                '  3.Lot Result
                '  4.Start_day �J�n�� �ݒ�ς�
                '  5.Start_time �J�n���� �ݒ�ς�
                '  6.End_day �I���� �ݒ�ς�
                '  7.End_time �I������ �ݒ�ς�
                '  8.Prod_time �J�n�`�I���܂łɗv��������
                If (.STtime > .EDtime) Then
                    UpTime = Double.Parse(24 * 60 * 60)                 ' Uptime = 1���̕b�� 
                End If
                TotalTime = (.EDtime + UpTime) - .STtime                ' ���v����(sec) = �I�����ԁ|�J�n����
                Call Cnv_SecToHHMMSS(TotalTime, .Prod_time)             ' .Prod_time = "00:00:00"
                '  9.�󔒍s
                ' 10.Lot_No QR�R�[�h���̃��b�g�ԍ�
                .Lot_No = gsQRInfo(4)
                .Lot_No = .Lot_No.PadLeft(PRT_LOT)

                ' 11.Prot_Typ �i��
                .Prot_Typ = gsQRInfo(0)                                 ' �^�C�v
                .Prot_Typ = .Prot_Typ.PadLeft(PRT_SZ)

                ' 12.IEC_Tol �ڕW��R�l(��1��R�̖ڕW�l)
                Call Cnv_TergetVal(typResistorInfoArray(1).dblTrimTargetVal, .IEC_Tol)
                .IEC_Tol = .IEC_Tol.PadLeft(PRT_SZ)

                ' 13.Cal_RV �ڕW��R�l(��1��R��P�J�b�g�̃J�b�g�I�t���l�������ڕW�l) 
                ' �J�b�g�I�t�l =  �ڕW��R�l * (1 + Cut off(%)/100)
                dblWK = 1 + typResistorInfoArray(1).ArrCut(1).dblCutOff * 0.01
                dblWK = typResistorInfoArray(1).dblTrimTargetVal * dblWK
                Call Cnv_TergetVal(dblWK, .Cal_RV)
                .Cal_RV = .Cal_RV.PadLeft(PRT_SZ - 1)

                ' 14.LaserPower ���H�ʏo��
                If (.LaserPower = "") Then
                    .LaserPower = "---"
                End If
                .LaserPower = .LaserPower.PadLeft(PRT_SZ)

                ' 15.Stop1 L�^�[���|�C���g(%)(��1��R��P�J�b�g��L�^�[���|�C���g) 
                If (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NL) Or (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NLr) Or _
                   (typResistorInfoArray(1).ArrCut(1).strCutType = CNS_CUTP_NLt) Then
                    .Stop1 = typResistorInfoArray(1).ArrCut(1).dblLTurnPoint.ToString("0.00") + "%"
                Else
                    .Stop1 = "---"
                End If
                .Stop1 = .Stop1.PadLeft(PRT_SZ)

                ' 16.Stop2 �J�b�g�I�t(%)
                .Stop2 = typResistorInfoArray(1).ArrCut(1).dblCutOff.ToString("0.00") + "%"
                .Stop2 = .Stop2.PadLeft(PRT_SZ)

                ' 17.Speed1 ���H���x1(L�^�[���O)
                .Speed1 = typResistorInfoArray(1).ArrCut(1).dblCutSpeed.ToString("0.0") + "mm/s"
                .Speed1 = .Speed1.PadLeft(PRT_SZ)

                ' 18.Speed2 ���H���x1(L�^�[����)
                Call Cnv_Spd2_Qrate2(0, .Speed2)
                .Speed2 = .Speed2.PadLeft(PRT_SZ)

                ' 19.Qrate1 Q���[�g1(L�^�[���O)
                iWK = typResistorInfoArray(1).ArrCut(1).CndNum(CUT_CND_L1)
                .Qrate1 = stCND.Freq(iWK).ToString("0.0") + "kHz"
                '----- V6.0.3.0_45 �� -----
                If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then      ' FL�łȂ� ?
                    .Qrate1 = typResistorInfoArray(1).ArrCut(1).dblQRate.ToString("0.0") + "kHz"
                End If
                '----- V6.0.3.0_45 �� -----
                .Qrate1 = .Qrate1.PadLeft(PRT_SZ)

                ' 20.Qrate2 Q���[�g2(L�^�[����)
                Call Cnv_Spd2_Qrate2(1, .Qrate2)
                .Qrate2 = .Qrate2.PadLeft(PRT_SZ)

                ' 21.GlvPosX,Y �K���o�m�X�^�[�g�|�C���g(�J�b�g�ʒuXY)
                .GlvPosX = typResistorInfoArray(1).ArrCut(1).dblStartPointX.ToString("0.000")
                .GlvPosX = .GlvPosX.PadLeft(6)
                .GlvPosY = typResistorInfoArray(1).ArrCut(1).dblStartPointY.ToString("0.000")
                .GlvPosY = .GlvPosY.PadLeft(6)

                ' 22.CompOffset �J�b�g�I�t(�����e�s���̂��ߖ��Ή����J�b�g�I�t(%)��ݒ�))
                .CompOffset = typResistorInfoArray(1).ArrCut(1).dblCutOff.ToString("0.00") + "%"
                .CompOffset = .CompOffset.PadLeft(PRT_SZ)

                ' 23.�󔒍s

                ' 24.TrimRate ���ϐ؏㗦(���ϒ�R�l/�ڕW��R�l)
                If (.Trim_TotalValCnt <> 0) Then                        ' Mean_Value = ���ݸނ��ꂽ���߂̕��ϒ�R�l
                Else
                    .Trim_TotalValCnt = 1
                End If
                dblWK = (.Trim_TotalVal / .Trim_TotalValCnt) * .Trim_TotalValKT
                .TrimRate = (dblWK / typResistorInfoArray(1).dblTrimTargetVal) * 100
                .sTrimRate = (.TrimRate.ToString("0.00") + "%").PadLeft(PRT_SZ)
                '----- V6.0.3.0_26�� -----
                .TrimRate = GetAverageFTValue()
                .TrimRate = .TrimRate / typResistorInfoArray(1).dblTrimTargetVal * 100.0
                .sTrimRate = (.TrimRate.ToString("0.00") + "%").PadLeft(PRT_SZ)
                '----- V6.0.3.0_26�� -----

                ' 25.MTBF �G���[�����Ԋu(���ό̏�Ԋu = �ғ����� / �װє�����)
                OpeTime = TotalTime - .AlmTotaltime                     ' �ғ����� = ���v���ԁ|�װђ�~����
                'V4.7.0.0�@��
                If (gSysPrm.stCTM.giSPECIAL = customROHM) Then          ' ���[���a����(SL436R/SL436S) ?
                    KosyoIntv = OpeTime / (.AlmCnt + 1)                 ' ���ό̏�Ԋu = �ғ����� / �װє�����
                Else
                    'V4.7.0.0�@��
                    If (.AlmCnt <> 0) Then
                        KosyoIntv = OpeTime / .AlmCnt                   ' ���ό̏�Ԋu = �ғ����� / �װє�����
                    Else
                        KosyoIntv = 0
                    End If
                End If                                                  ' V4.7.0.0�@
                Call Cnv_SecToHHMMSS(KosyoIntv, .MTBF)                  ' MTBF = "00:00:00"
                .MTBF = .MTBF.PadLeft(PRT_SZ)

                ' 26.MTTR �G���[������̕�������(���ϕ������� = �װђ�~İ�َ��� / �װє�����)
                If (.AlmCnt <> 0) Then
                    RecovTime = .AlmTotaltime / .AlmCnt                 ' ���ϕ������� = �װђ�~İ�َ��� / �װє�����
                Else
                    RecovTime = 0
                End If
                Call Cnv_SecToHHMMSS(RecovTime, .MTTR)                  ' MTTR = "00:00:00"
                .MTTR = .MTTR.PadLeft(PRT_SZ)

                ' 27.Tol_Sheet �g�[�^������
                .Tol_Sheet = m_lPlateCount                              ' �g�[�^������ = �v���[�g������
                .sTol_Sheet = (.Tol_Sheet.ToString("0")).PadLeft(PRT_SZ)

                ' 28.NG�L�����Z����(���ݒ肳���P�[�X���Ȃ����ߖ��Ή�)

                ' 29.JudgeValue �s�������(�SNG��/�g���~���O��)
                .sJudgeValue = (.JudgeValue.ToString("0.00") + "%").PadLeft(PRT_SZ)

                ' 30.OffsetX,Y �I�t�Z�b�g(BP�I�t�Z�b�gXY)
                .OffsetX = typPlateInfo.dblBpOffSetXDir.ToString("0.000")
                .OffsetX = .OffsetX.PadLeft(6)
                .OffsetY = typPlateInfo.dblBpOffSetYDir.ToString("0.000")
                .OffsetY = .OffsetY.PadLeft(6)

                ' 31.Prod_Count �g���~���O�`�b�v��
                .Prod_Count = m_lGoodCount + m_lNgCount                 ' �`�b�v��(m_lGoodCount(�Ǖi��R��) + m_lNgCount(�s�ǒ�R��))
                .sProd_Count = (.Prod_Count.ToString("0")).PadLeft(PRT_SZ)

                ' 32.Tol_NG_Sheet ��NG�����
                .sTol_NG_Sheet = (.Tol_NG_Sheet.ToString("0")).PadLeft(PRT_SZ)

                ' 33.�󔒍s
                ' 34.PCS (%)

                ' 35.Pretest_Hi_Fail �����l����s�ǂ����ߐ� + %
                .Pretest_Hi_Fail = m_lITHINGCount                       ' IT HI NG��
                Call Cnv_PCS_Per(.Pretest_Hi_Fail, .Prod_Count, .sPretest_Hi_Fail)

                ' 36.Pretest_Lo_Fail �����l�����s�ǂ����ߐ� + %
                .Pretest_Lo_Fail = m_lITLONGCount                       ' IT LO NG��
                Call Cnv_PCS_Per(.Pretest_Lo_Fail, .Prod_Count, .sPretest_Lo_Fail)

                ' 37.Pretest_Open �����l����ݕs�ǂ����ߐ� + %
                .Pretest_Open = m_lITOVERCount                          ' IT���ް�ݼސ�
                Call Cnv_PCS_Per(.Pretest_Open, .Prod_Count, .sPretest_Open)

                ' 38.Final_test_Hi_Fail ���ݸތ�̏���s�ǂ����ߐ� + %
                .Final_test_Hi_Fail = m_lFTHINGCount                    ' FT HI NG��
                Call Cnv_PCS_Per(.Final_test_Hi_Fail, .Prod_Count, .sFinal_test_Hi_Fail)

                ' 39.Final_test_Lo_Fail ���ݸތ�̉����s�ǂ����ߐ� + %
                .Final_test_Lo_Fail = m_lFTLONGCount                  ' FT LO NG��
                Call Cnv_PCS_Per(.Final_test_Lo_Fail, .Prod_Count, .sFinal_test_Lo_Fail)

                ' 40.�󔒍s

                ' 41.sTrim_OK �Ǖi���ߐ� + %
                .Trim_OK = m_lGoodCount                                 ' m_lITHINGCount(IT HI NG��)��ݒ�
                Call Cnv_PCS_Per(.Trim_OK, .Prod_Count, .sTrim_OK)      ' .sTrim_OK = PCS +  %

                ' 42.Init_OK �C�j�V�����Ǖi���ߐ� + %
                ' (�g���~���O�`�b�v�� - (IT NG���ߐ� + FT NG���ߐ�))
                .Init_OK = .Prod_Count - (.Pretest_Lo_Fail + .Pretest_Hi_Fail + .Pretest_Open + .Final_test_Hi_Fail + .Final_test_Lo_Fail)
                Call Cnv_PCS_Per(.Init_OK, .Prod_Count, .sInit_OK)      ' .sInit_OK = PCS +  %

                ' 43.Total_OK ���Ǖi���ߐ� + %(�Ǖi���ߐ��Ɠ����l)
                .Total_OK = .Trim_OK
                Call Cnv_PCS_Per(.Total_OK, .Prod_Count, .sTotal_OK)    ' .sTotal_OK = PCS +  %

                ' 44.Int_RV_NG �����l����s�ǂ����ߐ� + �����l�����s�ǂ����ߐ�
                .Int_RV_NG = .Pretest_Lo_Fail + .Pretest_Hi_Fail
                Call Cnv_PCS_Per(.Int_RV_NG, .Prod_Count, .sInt_RV_NG)  ' .sTInt_RV_NG = PCS +  %

                ' 45.AT_Trim_NG �SNG�`�b�v��
                .AT_Trim_NG = .Pretest_Lo_Fail + .Pretest_Hi_Fail + .Pretest_Open
                .AT_Trim_NG = .AT_Trim_NG + .Final_test_Hi_Fail + .Final_test_Lo_Fail
                Call Cnv_PCS_Per(.AT_Trim_NG, .Prod_Count, .sAT_Trim_NG) ' .sTInt_RV_NG = PCS +  %

                ' 46.�󔒍s

                ' 47.AlmCnt �A���[�������� �ݒ�ς�
                .sAlmCnt = (.AlmCnt.ToString("0")).PadLeft(PRT_SZ)

                ' 48.�󔒍s

                ' 49.�A���[����������/�G���[���e

            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimPrnData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O���ʌ��ʈ������"
    '''=========================================================================
    ''' <summary>�g���~���O���ʌ��ʈ������</summary>
    ''' <param name="Md">(INP)���[�h(0=�ʏ���, 1=Print�{�^������, 2=APP�I������)</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub PrnTrimResult(ByVal Md As Integer)

        Dim r As Integer
        Dim Idx As Integer
        Dim sLINE_DATA(ITEM_MAX) As String                              ' ����f�[�^
        Dim strHEAD As String
        Dim strDAT As String
        Dim strSPS As String = vbLf                                     ' �󔒍s(x0A) 
        Dim strMSG As String = ""

        Try
            '-------------------------------------------------------------------
            '   �����ݒ�
            '-------------------------------------------------------------------
            ' V4.0.0.0-60
            ' �V���v���g���}�̏ꍇ�ɂ́ALot���f�[�^�������Z�[�u����i���C����ʂ�DATA SAVE���������������s�j
            If gKeiTyp = KEY_TYPE_RS Then
                SubDataSave()
            End If
            ' V4.0.0.0-60

            ' ���[���a�����łȂ����NOP
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return

            ' �f�[�^�����[�h�Ȃ��NOP
            If (gLoadDTFlag = False) Then Return

            ' �蓮�^�]�Ȃ�NOP
            If (stPRT_ROHM.bAutoMode = False) Then Return

            ' ����p�̊J�n�����Z�b�g����Ă��Ȃ����NOP
            If (stPRT_ROHM.Start_day = "") Then Return

            '�uPrint ON/OFF�v�{�^�����uPrint OFF�v�Ȃ�NOP
            '----- V6.0.3.0_25�� -----
            'If (gSysPrm.stDEV.rPrnOut_flg = False) Then Return
            If (Form1.BtnPrintOnOff.BackColor <> Color.Lime) Then
                Return
            End If
            '----- V6.0.3.0_25�� -----

            ' APP�I����������CAll�̏ꍇ�A������̏ꍇ(APP�I���G���[�����Ƃ݂Ȃ�)�݈̂������
            If (Md = 2) Then                                            ' APP�I����������CAll ?
                If (stPRT_ROHM.bFlgPrint = True) Then Return '          ' ����ςȂ�NOP
                Call SetTrimEndTime()                                   ' �g���~���O�I�����Ԃ�ݒ肷��
            End If

            '-------------------------------------------------------------------
            ' �g���~���O���ʈ�����ڂ̃f�[�^���Z�b�g����(����O�ɐݒ�\�Ȃ���)
            '-------------------------------------------------------------------
            Call SetTrimPrnData()

            '-------------------------------------------------------------------
            '   ����p���o���ƈ�����ڂ�ݒ肷��
            '-------------------------------------------------------------------
            With stPRT_ROHM
                Idx = 1
                strHEAD = "------------------------"                                                                    '  --------------------------
                sLINE_DATA(Idx) = "Machine No:".PadRight(HED_SZ) + .MachineNo : Idx = Idx + 1                           '  1.���u�ԍ�
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              '  2.�󔒍s
                sLINE_DATA(Idx) = "Lot Result".PadRight(LINE_SZ) : Idx = Idx + 1                                        '  3.Lot Result
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                'sLINE_DATA(Idx) = "Start day:".PadRight(HED_SZ) + .Start_day : Idx = Idx + 1                           '  4.�J�n��(2014/Feb/20)�@V4.0.0.0-37
                sLINE_DATA(Idx) = "Start day:".PadRight(HED_SZ) + .Start_day.PadLeft(HED_S2) : Idx = Idx + 1            '  4.�J�n��(2014/Feb/20)�@V4.0.0.0-37
                sLINE_DATA(Idx) = "Start time:".PadRight(HED_SZ) + .Start_time.PadLeft(HED_S2) : Idx = Idx + 1          '  5.�J�n����
                'sLINE_DATA(Idx) = "End day:".PadRight(HED_SZ) + .End_day : Idx = Idx + 1                               '  6.�I����(2014/Feb/20)�@V4.0.0.0-37
                sLINE_DATA(Idx) = "End day:".PadRight(HED_SZ) + .End_day.PadLeft(HED_S2) : Idx = Idx + 1                '  6.�I����(2014/Feb/20)�@V4.0.0.0-37
                sLINE_DATA(Idx) = "End time:".PadRight(HED_SZ) + .End_time.PadLeft(HED_S2) : Idx = Idx + 1              '  7.�I������
                sLINE_DATA(Idx) = "Prod time:".PadRight(HED_SZ) + .Prod_time.PadLeft(HED_S2) : Idx = Idx + 1            '  8.�J�n�`�I���܂łɗv��������
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              '  9.�󔒍s
                sLINE_DATA(Idx) = "Lot No:".PadRight(HED_S3) + .Lot_No : Idx = Idx + 1                                  ' 10.QR�R�[�h���̃��b�g�ԍ�
                sLINE_DATA(Idx) = "Prod Type:".PadRight(HED_SZ) + .Prot_Typ : Idx = Idx + 1                             ' 11.�i��
                sLINE_DATA(Idx) = "IEC(Tol):".PadRight(HED_SZ) + .IEC_Tol : Idx = Idx + 1                               ' 12.�ڕW��R�l
                sLINE_DATA(Idx) = "Calculated RV:" + .Cal_RV : Idx = Idx + 1                                            ' 13.�ڕW��R�l(�J�b�g�I�t���l�������ڕW�l)
                sLINE_DATA(Idx) = "Laser Power:".PadRight(HED_SZ) + .LaserPower : Idx = Idx + 1                         ' 14.���H�ʏo��
                sLINE_DATA(Idx) = "Stop(%)1:".PadRight(HED_SZ) + .Stop1 : Idx = Idx + 1                                 ' 15.L�^�[���|�C���g(%)
                sLINE_DATA(Idx) = "Stop(%)2:".PadRight(HED_SZ) + .Stop2 : Idx = Idx + 1                                 ' 16.�J�b�g�I�t(%)
                sLINE_DATA(Idx) = "Speed1:".PadRight(HED_SZ) + .Speed1 : Idx = Idx + 1                                  ' 17.���H���x1(L�^�[���O)
                sLINE_DATA(Idx) = "Speed2:".PadRight(HED_SZ) + .Speed2 : Idx = Idx + 1                                  ' 18.���H���x2(L�^�[����)
                sLINE_DATA(Idx) = "Q-Rate1:".PadRight(HED_SZ) + .Qrate1 : Idx = Idx + 1                                 ' 19.Q���[�g1(L�^�[���O)
                sLINE_DATA(Idx) = "Q-Rate2:".PadRight(HED_SZ) + .Qrate2 : Idx = Idx + 1                                 ' 20.Q���[�g2(L�^�[����)
                sLINE_DATA(Idx) = "Glv posX:" + .GlvPosX + " Y:" + .GlvPosY : Idx = Idx + 1                             ' 21.�K���o�m�X�^�[�g�|�C���g(�J�b�g�ʒuXY)
                sLINE_DATA(Idx) = "Comp.Offset:".PadRight(HED_SZ) + .CompOffset : Idx = Idx + 1                         ' 22.�J�b�g�I�t(�����e�s���̂��ߖ��Ή����J�b�g�I�t(%)��ݒ�)
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 23.�󔒍s
                sLINE_DATA(Idx) = "Trim Rate:".PadRight(HED_SZ) + .sTrimRate : Idx = Idx + 1                            ' 24.���ϐ؏㗦
                sLINE_DATA(Idx) = "MTBF:".PadRight(HED_SZ) + .MTBF : Idx = Idx + 1                                      ' 25.�G���[�����Ԋu
                sLINE_DATA(Idx) = "MTTR:".PadRight(HED_SZ) + .MTTR : Idx = Idx + 1                                      ' 26.�G���[������̕�������
                sLINE_DATA(Idx) = "Tol.Sheet:".PadRight(HED_SZ) + .sTol_Sheet : Idx = Idx + 1                           ' 27.�g�[�^������
                sLINE_DATA(Idx) = "NG Cancel:".PadRight(HED_SZ) + "---".PadLeft(HED_S2) : Idx = Idx + 1                 ' 28.NG�L�����Z����(���ݒ肳���P�[�X���Ȃ����ߖ��Ή�)
                sLINE_DATA(Idx) = "JudgeValue:".PadRight(HED_SZ) + .sJudgeValue : Idx = Idx + 1                         ' 29.�s�������(�SNG��/�g���~���O��)
                sLINE_DATA(Idx) = "Offset X:" + .OffsetX + " Y:" + .OffsetY : Idx = Idx + 1                             ' 30.�I�t�Z�b�g(BP�I�t�Z�b�gXY)
                sLINE_DATA(Idx) = "Prod.Count:".PadRight(HED_SZ) + .sProd_Count : Idx = Idx + 1                         ' 31.�g���~���O�`�b�v��
                sLINE_DATA(Idx) = "Tol.NG sheet:".PadRight(HED_SZ) + .sTol_NG_Sheet : Idx = Idx + 1                     ' 32.��NG�����
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 32.�󔒍s
                sLINE_DATA(Idx) = "PCS".PadLeft(PRT_SZ + PRT_S2) + "(%)".PadLeft(PRT_S3) : Idx = Idx + 1                ' 34. PCS (%) �� ���o��
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = "PT over".PadRight(HED_S2) + .sPretest_Hi_Fail : Idx = Idx + 1                        ' 35.�����l����s�ǂ����ߐ�
                sLINE_DATA(Idx) = "PT under".PadRight(HED_S2) + .sPretest_Lo_Fail : Idx = Idx + 1                       ' 36.�����l�����s�ǂ����ߐ�
                sLINE_DATA(Idx) = "Probe Err".PadRight(HED_S2) + .sPretest_Open : Idx = Idx + 1                         ' 37.�����l����ݕs�ǂ����ߐ�
                sLINE_DATA(Idx) = "AT over".PadRight(HED_S2) + .sFinal_test_Hi_Fail : Idx = Idx + 1                     ' 38.���ݸތ�̏���s�ǂ����ߐ�
                sLINE_DATA(Idx) = "AT under".PadRight(HED_S2) + .sFinal_test_Lo_Fail : Idx = Idx + 1                    ' 39.���ݸތ�̉����s�ǂ����ߐ�
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 40.�󔒍s
                sLINE_DATA(Idx) = "Trim OK:".PadRight(HED_S2) + .sTrim_OK : Idx = Idx + 1                               ' 41.�Ǖi���ߐ�
                sLINE_DATA(Idx) = "Int OK:".PadRight(HED_S2) + .sInit_OK : Idx = Idx + 1                                ' 42.�C�j�V�����Ǖi���ߐ�
                sLINE_DATA(Idx) = "Total OK:".PadRight(HED_S2) + .sTotal_OK : Idx = Idx + 1                             ' 43.���Ǖi���ߐ�(�Ǖi���ߐ��Ɠ���)
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
                sLINE_DATA(Idx) = "TInt RV NG:".PadRight(HED_S2) + .sInt_RV_NG : Idx = Idx + 1                          ' 44.�C�j�V����NG�`�b�v�� + �t�@�C�i��NG�`�b�v��
                sLINE_DATA(Idx) = "AT Trim NG:".PadRight(HED_S2) + .sAT_Trim_NG : Idx = Idx + 1                         ' 45.�SNG�`�b�v��
                sLINE_DATA(Idx) = strSPS.PadRight(LINE_SZ) : Idx = Idx + 1                                              ' 46.�󔒍s
                sLINE_DATA(Idx) = "Alarm Count:".PadRight(HED_SZ) + .sAlmCnt : Idx = Idx + 1                            ' 47.�A���[��������
                sLINE_DATA(Idx) = " ".PadRight(LINE_SZ) : Idx = Idx + 1                                                 ' 48.�󔒍s
                sLINE_DATA(Idx) = "Alarm Time/Error Dtls:".PadRight(LINE_SZ) : Idx = Idx + 1                            ' 49.�A���[����������/�G���[���e
                sLINE_DATA(Idx) = strHEAD : Idx = Idx + 1                                                               '  --------------------------
            End With

            '-------------------------------------------------------------------
            '   ����p�e�L�X�g�{�b�N�X(TxtBoxPrint(��\��))�ɏo�͂���
            '-------------------------------------------------------------------
            ' ����p�e�L�X�g�{�b�N�X�Ƀg���~���O���ʂ��o�͂���
            For Idx = 1 To ITEM_MAX                                     ' �ő區�ڐ����J��Ԃ�
                strDAT = sLINE_DATA(Idx) & vbCrLf
                ' ����p�e�L�X�g�{�b�N�X�ɏo��
                If (Idx = 1) Then
                    Form1.TxtBoxPrint.Text = strDAT
                Else
                    Form1.TxtBoxPrint.AppendText(strDAT)
                End If
            Next Idx

            ' �A���[���f�[�^������p�e�L�X�g�{�b�N�X�ɏo�͂���
            Call ReadAlarmData(gFPATH_QR_ALARM)

            '-------------------------------------------------------------------
            '   �������(�e�L�X�g�{�b�N�X(TxtBoxPrint)�̕�������������)
            '-------------------------------------------------------------------
            r = Print_Data(Form1.TxtBoxPrint.Text, strHEAD, HEAD_POSX, HEAD_POSY)
            If (r <> cFRS_NORMAL) Then
                ' "��������Ɏ��s���܂����B(r=xxxx)"
                strMSG = MSG_153 + "(r = " + r.ToString("0") + ")"
                Call MsgBox(strMSG, vbOKOnly)
            End If
            stPRT_ROHM.bFlgPrint = True                                 ' ����ς݃t���O = True(�����)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.PrnTrimResult() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '---------------------------------------------------------------------------
    '   ���ʊ֐�
    '---------------------------------------------------------------------------
#Region "�g���~���O�J�n���Ԃ�ݒ肷��"
    '''=========================================================================
    ''' <summary>�g���~���O�J�n���Ԃ�ݒ肷��</summary>
    ''' <remarks>�ȉ�����Call�����
    ''' �@�@�@ �ETrimming(�g���~���O���s��)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetTrimStartTime()

        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strYYYY As String
        Dim strMM As String
        Dim strDD As String
        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            If (stPRT_ROHM.Start_time <> "") Then Return '              ' ���łɐݒ�ς݂Ȃ�NOP

            ' ������ڂ�ݒ肷��
            With stPRT_ROHM
                .Start_time = TimeOfDay.ToString("HH:mm:ss")            ' �J�n����(hh:mm:ss)
                .STtime = TimeOfDt.Ticks / 10000000                     ' �J�n����(���݂̎������ߑO0������̒ʎZ100�i�m�b�ŕԂ����b)
                '.Start_day = Today.ToString("yyyy/MM/dd")              ' �J�n��(2014/02/05)
                strYYYY = DateTime.Today.Year.ToString("0000")          ' ��Ɠ�(2014/Feb/5)
                strDD = DateTime.Today.Day.ToString("0")                ' 
                Dim Cl As New System.Globalization.CultureInfo("en-US") ' (�J���`���[���p��ɂ���) 
                strMM = Dt.ToString("MMM", Cl)                          ' (���̏ȗ��`(February��Feb)) 
                .Start_day = strYYYY + "/" + strMM + "/" + strDD        ' �J�n��(2014/Feb/5)
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�g���~���O�I�����Ԃ�ݒ肷��"
    '''=========================================================================
    ''' <summary>�g���~���O�I�����Ԃ�ݒ肷��</summary>
    ''' <remarks>�ȉ�����Call�����
    ''' �@�@�@ �ETrimming(�����^�]�I����)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetTrimEndTime()

        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strYYYY As String
        Dim strMM As String
        Dim strDD As String
        Dim strMSG As String

        Try
            ' ������ڂ�ݒ肷��
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            With stPRT_ROHM
                .End_time = TimeOfDay.ToString("HH:mm:ss")              ' �I������(hh:mm:ss)
                .EDtime = TimeOfDt.Ticks / 10000000                     ' �I������(���݂̎������ߑO0������̒ʎZ100�i�m�b�ŕԂ����b)
                strYYYY = DateTime.Today.Year.ToString("0000")          ' ��Ɠ�(2014/Feb/5)
                strDD = DateTime.Today.Day.ToString("0")                ' 
                Dim Cl As New System.Globalization.CultureInfo("en-US") ' (�J���`���[���p��ɂ���) 
                strMM = Dt.ToString("MMM", Cl)                          ' (���̏ȗ��`(February��Feb)) 
                .End_day = strYYYY + "/" + strMM + "/" + strDD          ' �I����(2014/Feb/5)
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.SetTrimEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��"
    '''=========================================================================
    ''' <summary>�A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��</summary>
    ''' <remarks>�ȉ�����Call�����
    ''' �@�@�@ �EFormLoaderAlarm.ShowDialog(���[�_�A���[���\���J�n��)
    ''' �@�@�@ �ECall_SetAlmStartTime(OcxSystem(INtime���G���[�\���J�n��)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetAlmStartTime()

        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            If (stPRT_ROHM.bAutoMode = False) Then Return '             ' �����^�]���łȂ����NOP

            ' �A���[�������񐔂ƃA���[����~�J�n���Ԃ�ݒ肷��
            gbAlarmFlg = True                                           ' �A���[�������t���O = True(�A���[������)
            With stPRT_ROHM
                .AlarmST_time = TimeOfDay.ToString("HH:mm:ss")          ' �A���[���J�n����(hh:mm:ss)
                .AlmCnt = .AlmCnt + 1                                   ' �A���[��������
                .AlmSTtime = TimeOfDt.Ticks / 10000000                  ' �A���[����~�J�n����(���݂̎������ߑO0������̒ʎZ100�i�m�b�ŕԂ����b)
            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.SetAlmStartTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "�A���[����~�I�����Ԃ�ݒ肷��"
    '''=========================================================================
    ''' <summary>�A���[����~�I�����Ԃ�ݒ肷��</summary>
    ''' <remarks>�ȉ�����Call�����
    ''' �@�@�@ �ESub_CallFormLoaderAlarm(���[�_�A���[���\���I����)
    ''' �@�@�@ �ECall_SetAlmEndTime(OcxSystem(INtime���G���[�\���I����)
    ''' </remarks>
    '''=========================================================================
    Public Sub SetAlmEndTime()

        Dim Dt As DateTime = DateTime.Now                               ' ���݂̓������擾
        Dim TimeOfDt As TimeSpan = Dt.TimeOfDay                         ' ���݂̎����݂̂��擾 
        Dim UpTime As Double = 0.0
        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            If (gbAlarmFlg = False) Then Return '                       ' �A���[�������łȂ����NOP
            gbAlarmFlg = False

            ' �A���[����~�I�����Ԃ�ݒ肷��
            With stPRT_ROHM
                .AlmEDtime = TimeOfDt.Ticks / 10000000                  ' �A���[����~�I������(���݂̎������ߑO0������̒ʎZ100�i�m�b�ŕԂ����b)

                ' �A���[����~�g�[�^������
                If (.AlmSTtime > .AlmEDtime) Then
                    UpTime = Double.Parse(24 * 60 * 60)                 ' Uptime = 1���̕b�� 
                End If
                .AlmTotaltime = (.AlmEDtime + UpTime) - .AlmSTtime      ' �I�����ԁ|�J�n����

            End With

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.SetAlmEndTime() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�f�[�^�ҏW(�b�����u��:��:�b�v�`���ɕϊ����ĕԂ�)"
    '''=========================================================================
    ''' <summary>�b�����u��:��:�b�v�`���ɕϊ����ĕԂ�</summary>
    ''' <param name="dblSec">(INP)�b��</param>
    ''' <param name="strDAT">(OUT)�u��:��:�b�v�ɕϊ������l</param>
    '''=========================================================================
    Public Sub Cnv_SecToHHMMSS(ByVal dblSec As Double, ByRef strDAT As String)

        Dim wkHH As Integer
        Dim wkMM As Integer
        Dim wkSS As Integer
        Dim strMSG As String

        Try
            wkHH = dblSec \ 3600                                       ' HH = 3600�b(1����)�Ŋ������]�� 
            wkMM = (dblSec Mod 3600) \ 60                              ' MM = (3600�b(1����)�Ŋ������l) ��60�b(1��)�Ŋ������]�� 
            wkSS = (dblSec Mod 3600) Mod 60                            ' SS = (3600�b(1����)�Ŋ������l) ��60�b(1��)�Ŋ������l 

            strDAT = wkHH.ToString("00") + ":" + wkMM.ToString("00") + ":" + wkSS.ToString("00")

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_SecToHHMMSS() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�f�[�^�ҏW(����%���u     9    99.99�v�`���ɕϊ����ĕԂ�)"
    '''=========================================================================
    ''' <summary>����%���u     9    99.99�v�`���ɕϊ����ĕԂ�</summary>
    ''' <param name="Count">   (INP)��</param>
    ''' <param name="TtlCount">(INP)����</param>
    ''' <param name="strDAT">�@(OUT)�u     9    99.99�v�ɕϊ������l</param>
    '''=========================================================================
    Public Sub Cnv_PCS_Per(ByVal Count As Integer, ByVal TtlCount As Integer, ByRef strDAT As String)

        Dim strMSG As String

        Try
            strDAT = (Count.ToString("0")).PadLeft(PRT_S2)                              ' �� = �E�l�O����
            If (TtlCount = 0) Then
                strMSG = "0.00".PadLeft(PRT_S3)                                         ' %   = �E�l�O����
            Else
                strMSG = (((Count / TtlCount) * 100).ToString("0.00")).PadLeft(PRT_S3)  ' %   = �E�l�O����
            End If
            strDAT = strDAT + strMSG                                                    ' "     ��     %"

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_PCS_Per() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�f�[�^�ҏW(���x2��Q���[�g2��ҏW���ĕԂ�)"
    '''=========================================================================
    ''' <summary>���x2��Q���[�g2��ҏW���ĕԂ�</summary>
    ''' <param name="Kd">    (INP)0=���x2, 1=Q���[�g2</param>
    ''' <param name="strDAT">(OUT)�ϊ������l</param>
    '''=========================================================================
    Public Sub Cnv_Spd2_Qrate2(ByVal Kd As Integer, ByRef strDAT As String)

        Dim iWK As Integer
        Dim strMSG As String

        Try
            '----- V6.0.3.0_45 �� -----
            If (gSysPrm.stRAT.giOsc_Res <> OSCILLATOR_FL) Then           ' FL�łȂ� ?
                ' ��1��R��P�J�b�g�̎�ʂɂ��ݒ肷��
                Select Case typResistorInfoArray(1).ArrCut(1).strCutType
                    ' �΂�ST�J�b�g(���^�[��, ���g���[�X), �΂�L�J�b�g(���^�[��, ���g���[�X)
                    Case CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_NLr, CNS_CUTP_NLt
                        If (Kd = 0) Then
                            strDAT = typResistorInfoArray(1).ArrCut(1).dblCutSpeed2.ToString("0.0") + "mm/s"
                        Else
                            strDAT = typResistorInfoArray(1).ArrCut(1).dblQRate2.ToString("0.0") + "kHz"
                        End If
                    Case Else
                        strDAT = "---"
                End Select
                Return
            End If
            '----- V6.0.3.0_45 �� -----

            ' ��1��R��P�J�b�g�̎�ʂɂ��ݒ肷��
            Select Case typResistorInfoArray(1).ArrCut(1).strCutType
                ' �΂�ST�J�b�g(���^�[��, ���g���[�X), �΂�L�J�b�g, �΂�L�J�b�g(���^�[��, ���g���[�X), HK�J�b�g, U�J�b�g
                Case CNS_CUTP_NSTr, CNS_CUTP_NSTt, CNS_CUTP_NL, CNS_CUTP_NLr, CNS_CUTP_NLt, CNS_CUTP_HK, CNS_CUTP_U, CNS_CUTP_Ut ' V1.22.0.0�@
                    If (Kd = 0) Then
                        strDAT = typResistorInfoArray(1).ArrCut(1).dblCutSpeed2.ToString("0.0") + "mm/s"
                    Else
                        iWK = typResistorInfoArray(1).ArrCut(1).CndNum(CUT_CND_L2)
                        strDAT = stCND.Freq(iWK).ToString("0.0") + "kHz"
                    End If

                Case Else
                    strDAT = "---"
            End Select

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_Spd2_Qrate2() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�f�[�^�ҏW(�ڕW�l��ҏW���ĕԂ�)"
    '''=========================================================================
    ''' <summary>�ڕW�l��ҏW���ĕԂ�</summary>
    ''' <param name="Nominal">(INP)�ڕW�l</param>
    ''' <param name="strDAT"> (OUT)�u��:��:�b�v�ɕϊ������l</param>
    '''=========================================================================
    Public Sub Cnv_TergetVal(ByVal Nominal As Double, ByRef strDAT As String)

        Dim dblWK As Double
        Dim strMSG As String

        Try
            If (Nominal < 1.0) Then
                dblWK = Nominal * 1000
                strDAT = dblWK.ToString("0.000") + "m"
            ElseIf (Nominal >= 1.0) And (Nominal < 1000.0) Then
                strDAT = Nominal.ToString("0.000")
            ElseIf (Nominal >= 1000.0) And (Nominal < 1000000.0) Then
                dblWK = Nominal * 0.001
                strDAT = dblWK.ToString("0.000") + "K"
            ElseIf (Nominal >= 1000000.0) And (Nominal < 1000000000.0) Then
                dblWK = Nominal * 0.000001
                strDAT = dblWK.ToString("0.000") + "M"
            Else
                dblWK = Nominal * 0.000000001
                strDAT = dblWK.ToString("0.000") + "G"
            End If

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.Cnv_TergetVal() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�A���[���t�@�C���ɃA���[���f�[�^����������"
    '''=========================================================================
    ''' <summary>����p�A���[���t�@�C���ɃA���[���f�[�^����������</summary>
    ''' <param name="strPath"> (INP)�t�@�C���p�X��</param>
    ''' <param name="strDAT">  (INP)�G���[���b�Z�[�W</param>
    ''' <param name="strTIME"> (INP)�A���[����������("hh:mm:ss")</param>
    ''' <param name="ErrCode"> (INP)�G���[�R�[�h</param>
    '''=========================================================================
    Public Sub WriteAlarmData(ByVal strPath As String, ByVal strDAT As String, ByVal strTIME As String, ByVal ErrCode As Short)

        'Dim writer As System.IO.StreamWriter = Nothing
        Dim strMSG As String

        Try
            ' ��������
            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return '    ' ���[���a�����łȂ����NOP
            '                                                           ' false = �㏑��(true = �ǉ�)
            Return  'V1.18.0.0�L �L���ɂ���ɂ͂��̍s���폜����

            'writer = New System.IO.StreamWriter(strPath, True, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Using writer As New StreamWriter(strPath, True, Encoding.UTF8)      'V4.4.0.0-0
                ' �A���[���f�[�^��ҏW���ăt�@�C���ɏ�������
                ' "HH:MM:SS,ERROR:,xxxxxx,���b�Z�[�W" + \r\n
                strMSG = strTIME & ",ERROR,"                                    ' ��������(hh:mm:ss)
                strMSG = strMSG & System.Math.Abs(ErrCode).ToString("0") & ","  ' �G���[�R�[�h
                strMSG = strMSG & strDAT                                        ' ���b�Z�[�W 
                writer.WriteLine(strMSG)

                ' �㏈��
                'writer.Close()
            End Using

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.WriteAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�A���[���t�@�C���̃A���[���f�[�^������p�e�L�X�g�{�b�N�X�ɏo�͂���"
    '''=========================================================================
    ''' <summary>����p�A���[���t�@�C���̃A���[���f�[�^������p�e�L�X�g�{�b�N�X�ɏo�͂���</summary>
    ''' <param name="strPath"> (INP)�t�@�C���p�X��</param>
    '''=========================================================================
    Public Sub ReadAlarmData(ByVal strPath As String)

        'Dim intFileNo As Integer                                        ' �t�@�C���ԍ�
        'Dim iFlg As Integer
        Dim Ln As Integer
        Dim Sz As Integer
        Dim Pos As Integer
        Dim strDAT As String                                            ' �ǂݍ��݃f�[�^�o�b�t�@
        Dim strMSG As String
        Dim mDATA() As String = New String(4) {}

        Try
            '-------------------------------------------------------------------
            '   ��������
            '-------------------------------------------------------------------
            ' �A���[���t�@�C�������݂��Ȃ����NOP
            If (System.IO.File.Exists(strPath) = False) Then Return

            ' �A���[���t�@�C�����I�[�v������
            'intFileNo = FreeFile()                                      ' �g�p�\�ȃt�@�C���i���o�[���擾
            'FileOpen(intFileNo, strPath, OpenMode.Input)
            'iFlg = 1
            Using sr As New StreamReader(strPath, Encoding.UTF8)        'V4.4.0.0-0
                '-------------------------------------------------------------------
                '   �A���[���f�[�^������p�e�L�X�g�{�b�N�X�ɏo�͂���
                '-------------------------------------------------------------------
                ' �t�@�C���̏I�[�܂Ń��[�v���J��Ԃ�
                'Do While Not EOF(intFileNo)
                Do While (False = sr.EndOfStream)                       'V4.4.0.0-0
                    'strDAT = LineInput(intFileNo)                           ' 1�s�ǂݍ���
                    strDAT = sr.ReadLine()                                  ' 1�s�ǂݍ���
                    If (strDAT.Length < 20) Then GoTo STP_NEXT

                    ' "HH:MM:SS,ERROR:,xxxxxx" + \r\n
                    mDATA = strDAT.Split(",")                               ' �f�[�^��,��؂�Ŏ��o��
                    strMSG = mDATA(0).PadRight(12)                          ' "HH:MM:SS    "
                    strMSG = strMSG + (mDATA(1) + ":")                      ' "ERROR:"
                    strMSG = strMSG + mDATA(2).PadLeft(6) + vbCrLf          ' "  �G���[�R�[�h"
                    Form1.TxtBoxPrint.AppendText(strMSG)                    ' ����p�e�L�X�g�{�b�N�X�ɏo�͂���

                    ' ���b�Z�[�W��1�s�̕��������Â��o��
                    Sz = mDATA(3).Length
                    Ln = mDATA(3).Length
                    Pos = 0
                    Do While (Pos < Ln)
                        If (Sz > LINE_SZ) Then
                            Sz = LINE_SZ
                        Else
                            Sz = Ln - Pos
                        End If
                        strMSG = mDATA(3).Substring(Pos, Sz)                ' ���b�Z�[�W
                        strMSG = strMSG.PadRight(LINE_SZ) + vbCrLf          ' 
                        Form1.TxtBoxPrint.AppendText(strMSG)                ' ����p�e�L�X�g�{�b�N�X�ɏo�͂���
                        Pos = Pos + Sz
                    Loop

                    'If (gSysPrm.stTMN.giMsgTyp = 0) Then                    ' ���{��(�{���͉p��̂�) ? 
                    '    Sz = mDATA(3).Length
                    '    Ln = mDATA(3).Length
                    '    Pos = 0
                    '    Do While (Pos < Ln)
                    '        If (Sz > (LINE_SZ / 2)) Then
                    '            Sz = LINE_SZ / 2
                    '        Else
                    '            Sz = Ln - Pos
                    '        End If
                    '        strMSG = mDATA(3).Substring(Pos, Sz) + vbCrLf   ' ���b�Z�[�W
                    '        Form1.TxtBoxPrint.AppendText(strMSG)            ' ����p�e�L�X�g�{�b�N�X�ɏo�͂���
                    '        Pos = Pos + Sz
                    '    Loop

                    'Else                                                    ' �p��̏ꍇ 
                    '    Sz = mDATA(3).Length
                    '    Ln = mDATA(3).Length
                    '    Pos = 0
                    '    Do While (Pos < Ln)
                    '        If (Sz > LINE_SZ) Then
                    '            Sz = LINE_SZ
                    '        Else
                    '            Sz = Ln - Pos
                    '        End If
                    '        strMSG = mDATA(3).Substring(Pos, Sz) + vbCrLf   ' ���b�Z�[�W
                    '        Form1.TxtBoxPrint.AppendText(strMSG)            ' ����p�e�L�X�g�{�b�N�X�ɏo�͂���
                    '        Pos = Pos + Sz
                    '    Loop
                    'End If

STP_NEXT:
                Loop

                '-------------------------------------------------------------------
                '   �㏈��
                '-------------------------------------------------------------------
                'strMSG = " ".PadRight(LINE_SZ) + vbCr + vbCr + vbCr + vbCrLf
                'Form1.TxtBoxPrint.AppendText(strMSG)                        ' �_�~�[���

                'If (iFlg = 1) Then
                '    FileClose(intFileNo)                                    ' �t�@�C���N���[�Y 
                'End If

            End Using

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.ReadAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "����p�A���[���t�@�C�����폜����"
    '''=========================================================================
    ''' <summary>����p�A���[���t�@�C�����폜����</summary>
    ''' <param name="strPath"> (INP)�t�@�C���p�X��</param>
    '''=========================================================================
    Public Sub DeleteAlarmData(ByVal strPath As String)

        Dim strMSG As String = ""

        Try
            ' �A���[���t�@�C�������݂��Ȃ����NOP
            If (System.IO.File.Exists(strPath) = False) Then Return
            ' �����^�]�p�����Ȃ�NOP   
            If (gbFgContinue = True) Then Return

            ' �A���[���t�@�C�����폜����
            System.IO.File.Delete(strPath)                              ' �t�@�C�������݂��Ȃ��Ă���O�͔������Ȃ����g�p���̏ꍇ�͔�������

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "QR_Print.DeleteAlarmData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '-------------------------------------------------------------------------------
    '   �r�����g�p�����ʏ�̃v�����^�̈����   ��
    '-------------------------------------------------------------------------------
#Region "�r�����g�p�����ʏ�̃v�����^�̈����"
    '#Region "�y�萔/�ϐ��̒�`�z"
    '    '===========================================================================
    '    '   �萔/�ϐ��̒�`
    '    '===========================================================================
    '    '----- �N���X�I�u�W�F�N�g -----
    '    Private ObjPrint As DllPrintString.PrintString = Nothing            ' ����p�I�u�W�F�N�g(DllPrintString.dll)
    '    'Private strFont As String = "�l�r �o�S�V�b�N"                     ' ����p�t�H���g 
    '    Private strFont As String = "�l�r �S�V�b�N"                       ' ����p�t�H���g 
    '    Private iFontSz As Integer = 10                                     ' �t�H���g�T�C�Y

    '    '----- �r������p -----
    'Private Const ITEM_MAX As Integer = 50                              ' ������ڐ�
    'Private Const H_LINE_NUM As Integer = ITEM_MAX                      ' �r�������C����(0-51)
    'Private Const V_LINE_NUM As Integer = (4 - 1)                       ' �r���c���C����(0-3)
    'Private KWidth As Double = 0.2                                      ' �r������p�y����(mm)
    'Private H_StaPosAry(,) As Integer = Nothing                         ' �r�������C���J�n���WX,Y�z��
    'Private H_EndPosAry(,) As Integer = Nothing                         ' �r�������C���I�����WX,Y�z��
    'Private V_StaPosAry(,) As Integer = Nothing                         ' �r���c���C���J�n���WX,Y�z��
    'Private V_EndPosAry(,) As Integer = Nothing                         ' �r���c���C���I�����WX,Y�z��

    '    '----- ����ʒu -----
    '    Private Const HEAD_POSX As Integer = 10                             ' ���o��������WX
    '    Private Const HEAD_POSY As Integer = 6                              ' ���o��������WY

    '    '----- ����ʒu(�r��(�c���C��)) -----
    '    Private Const PR_V0_POSX As Integer = 10                            ' �c���W0X
    '    Private Const PR_V1_POSX As Integer = 20                            ' �c���W1X
    '    Private Const PR_V2_POSX As Integer = 69                            ' �c���W2X
    '    Private Const PR_V3_POSX As Integer = 110                           ' �c���W3X

    '    Private Const PR_V0_POSY As Integer = 10                            ' �c���W0Y
    '    Private Const PR_V1_POSY As Integer = 120                           ' �c���W1Y

    '    '----- ����ʒu(�r��(�����C��)) -----
    '    Private Const PR_H0_POSX As Integer = PR_V0_POSX                    ' �c���W0X

    '    Private Const PR_H0_OFSY As Integer = 5                             ' �r���c�s�b�` 

    '    Private Const PR_H00_POSY As Integer = PR_V0_POSY                    ' �c���W0Y(10)
    '    Private Const PR_H01_POSY As Integer = PR_H00_POSY + PR_H0_OFSY      ' �c���W1Y(15)
    '    Private Const PR_H02_POSY As Integer = PR_H01_POSY + PR_H0_OFSY      ' �c���W2Y( .) 
    '    Private Const PR_H03_POSY As Integer = PR_H02_POSY + PR_H0_OFSY      ' �c���W3Y
    '    Private Const PR_H04_POSY As Integer = PR_H03_POSY + PR_H0_OFSY      ' �c���W4Y
    '    Private Const PR_H05_POSY As Integer = PR_H04_POSY + PR_H0_OFSY      ' �c���W5Y
    '    Private Const PR_H06_POSY As Integer = PR_H05_POSY + PR_H0_OFSY      ' �c���W6Y
    '    Private Const PR_H07_POSY As Integer = PR_H06_POSY + PR_H0_OFSY      ' �c���W7Y
    '    Private Const PR_H08_POSY As Integer = PR_H07_POSY + PR_H0_OFSY      ' �c���W8Y
    '    Private Const PR_H09_POSY As Integer = PR_H08_POSY + PR_H0_OFSY      ' �c���W9Y
    '    Private Const PR_H10_POSY As Integer = PR_H09_POSY + PR_H0_OFSY      ' �c���W10Y
    '    Private Const PR_H11_POSY As Integer = PR_H10_POSY + PR_H0_OFSY      ' �c���W11Y
    '    Private Const PR_H12_POSY As Integer = PR_H11_POSY + PR_H0_OFSY      ' �c���W12Y
    '    Private Const PR_H13_POSY As Integer = PR_H12_POSY + PR_H0_OFSY      ' �c���W13Y
    '    Private Const PR_H14_POSY As Integer = PR_H13_POSY + PR_H0_OFSY      ' �c���W14Y
    '    Private Const PR_H15_POSY As Integer = PR_H14_POSY + PR_H0_OFSY      ' �c���W15Y
    '    Private Const PR_H16_POSY As Integer = PR_H15_POSY + PR_H0_OFSY      ' �c���W16Y
    '    Private Const PR_H17_POSY As Integer = PR_H16_POSY + PR_H0_OFSY      ' �c���W17Y
    '    Private Const PR_H18_POSY As Integer = PR_H17_POSY + PR_H0_OFSY      ' �c���W18Y
    '    Private Const PR_H19_POSY As Integer = PR_H18_POSY + PR_H0_OFSY      ' �c���W19Y
    '    Private Const PR_H20_POSY As Integer = PR_H19_POSY + PR_H0_OFSY      ' �c���W20Y
    '    Private Const PR_H21_POSY As Integer = PR_H20_POSY + PR_H0_OFSY      ' �c���W21Y
    '    Private Const PR_H22_POSY As Integer = PR_H21_POSY + PR_H0_OFSY      ' �c���W22Y
    '    Private Const PR_H23_POSY As Integer = PR_H22_POSY + PR_H0_OFSY      ' �c���W23Y
    '    Private Const PR_H24_POSY As Integer = PR_H23_POSY + PR_H0_OFSY      ' �c���W24Y
    '    Private Const PR_H25_POSY As Integer = PR_H24_POSY + PR_H0_OFSY      ' �c���W25Y
    '    Private Const PR_H26_POSY As Integer = PR_H25_POSY + PR_H0_OFSY      ' �c���W26Y
    '    Private Const PR_H27_POSY As Integer = PR_H26_POSY + PR_H0_OFSY      ' �c���W27Y
    '    Private Const PR_H28_POSY As Integer = PR_H27_POSY + PR_H0_OFSY      ' �c���W28Y
    '    Private Const PR_H29_POSY As Integer = PR_H28_POSY + PR_H0_OFSY      ' �c���W29Y
    '    Private Const PR_H30_POSY As Integer = PR_H29_POSY + PR_H0_OFSY      ' �c���W30Y
    '    Private Const PR_H31_POSY As Integer = PR_H30_POSY + PR_H0_OFSY      ' �c���W31Y

    '    '---------------------------------------------------------------------------
    '    '   �g���~���O���ʈ���p�\����(���[���a�p) 
    '    '---------------------------------------------------------------------------
    '    Public Structure TRIM_RESULT_PRINT_ROHM_INFO
    '        Dim bAutoMode As Boolean                                        ' ����/�蓮(False=�蓮�^�], True = �����^�])
    '        Dim DateR As String                                             ' ��Ɠ�(2014/Feb/20)
    '        Dim START_TIME As String                                        ' �J�n����(hh:mm:ss)
    '        Dim STOP_TIME As String                                         ' �I������(hh:mm:ss)
    '        Dim PROG_TIME As String                                         ' �J�n�`�I���܂łɗv��������(hh:mm:ss)
    '        Dim OPE_TIME As String                                          ' �ғ�����(hh:mm:ss)
    '        Dim ALARM_TIME As String                                        ' �װтɂ���~��������(hh:mm:ss)
    '        Dim OPE_RATE As String                                          ' �ғ���(1-
    '        Dim MTBF As String                                              ' ���ό̏�Ԋu
    '        Dim MTTR As String                                              ' ���ϕ�������
    '        Dim LOT_NO As String                                            ' ���ݸ��ް����ݽ���ް(QR�R�[�h���̃��b�g�ԍ�)
    '        Dim Qrate As String                                             ' ���ݸ�Qڰ�
    '        Dim Trim_Speed As String                                        ' ���ݸ޶�Ľ�߰��
    '        Dim Trim_OK As Integer                                          ' �Ǖi���ߐ�
    '        Dim Pretest_Lo_Fail As Integer                                  ' �����l�����s�ǂ����ߐ�
    '        Dim Pretest_Hi_Fail As Integer                                  ' �����l����s�ǂ����ߐ�
    '        Dim Pretest_Open As Integer                                     ' �����l����ݕs�ǂ����ߐ�
    '        Dim Cut_NG As Integer                                           ' ���ݸގ��ɖڕW�l�ɒB���Ȃ��������ߐ�
    '        Dim Pretest_NG_Cut_NG As Integer                                ' �����s��
    '        Dim Final_test_Lo_Fail As Integer                               ' ���ݸތ�̉����s�ǂ����ߐ�
    '        Dim Final_test_Hi_Fail As Integer                               ' ���ݸތ�̏���s�ǂ����ߐ�
    '        Dim Final_test_Open As Integer                                  ' ���ݸތ�ɵ���ݴװ�ƂȂ������ߐ�
    '        Dim Yield As String                                             ' �Ǖi���ߐ������ߐ�
    '        Dim Yield_Par As Double                                         ' ��L��%�\��
    '        Dim Pdt_Sheet As Integer                                        ' ���ݸ޽ð�ނŏ������������
    '        Dim Lot_Sheet As Integer                                        ' ���u�ɓ������ꂽۯĖ���
    '        Dim Lot_NG_Sheet As Integer                                     ' ۯĒ��̕s�Ǌ��
    '        Dim Edg_Fail As Integer                                         ' ۯĒ��̔F���s�Ǌ����
    '        Dim Nominal As Double                                           ' �ڕW��R�l
    '        Dim Trim_Target As Double                                       ' �␳��̖ڕW��R�l(�ڕW�l*(1+��R��100)) ����R�w��Ȃ��̂��ߖ��g�p
    '        Dim Trim_Limit As Double                                        ' ���ݸޖڕW�␳�l ����R�w��Ȃ��̂��ߖ��g�p
    '        Dim Mean_Value As Double                                        ' ���ݸނ��ꂽ���߂̕��ϒ�R�l
    '        Dim _Par As Double                                              ' ��L��%�\��
    '        Dim M_R As Double                                               ' ���ϒl�̌덷
    '        Dim Prn3s_x As Double                                           ' ���ݸނ��ꂽ���߂̌덷�̕W���΍�
    '        Dim STtime As Double                                            ' �J�n����(double)
    '        Dim EDtime As Double                                            ' �I������(double)
    '        Dim AlmSTtime As Double                                         ' �װђ�~�J�n����(double)
    '        Dim AlmEDtime As Double                                         ' �װђ�~�I������(double)
    '        Dim AlmCnt As Short                                             ' �װє�����
    '        Dim AlmTotaltime As Double                                      ' �װђ�~İ�َ���(double)
    '        Dim ChipTotal As Double                                         ' 1ۯĕ��̑���R��
    '        Dim Trim_NG As Integer                                          ' �s�Ǖi���ߐ�
    '        Dim Trim_TotalVal As Double                                     ' ���ݸނ��ꂽ���߂̒�R�l(���v)
    '        Dim Trim_TotalValCnt As Double                                  ' ���ݸނ��ꂽ���߂̒�R�l(�v�Z�p���v)
    '        Dim Trim_TotalValKT As Short                                    ' ���ݸނ��ꂽ���߂̒�R�l(��)
    '    End Structure

    '    Public stPRT_ROHM As TRIM_RESULT_PRINT_ROHM_INFO                    ' �g���~���O���ʈ���p�\����(���[���a�p)

    '#End Region

    '#Region "�r���ʒu��ݒ肷��"
    '    '''=========================================================================
    '    ''' <summary>�r���ʒu��ݒ肷��</summary>
    '    ''' <param name="KWidth">     (INP)�r������p�y����(mm)</param>
    '    ''' <param name="H_StaPosAry">(INP)�����C���J�n���WX,Y�z��</param>
    '    ''' <param name="H_EndPosAry">(INP)�����C���I�����WX,Y�z��</param>
    '    ''' <param name="V_StaPosAry">(INP)�c���C���J�n���WX,Y�z��</param>
    '    ''' <param name="V_EndPosAry">(INP)�c���C���I�����WX,Y�z��</param>
    '    ''' <returns>0 = ����, 0�ȊO=�G���[</returns>
    '    '''=========================================================================
    '    Public Function Set_Keisen_Pos(ByVal KWidth As Double, ByVal H_StaPosAry(,) As Integer, ByVal H_EndPosAry(,) As Integer, ByVal V_StaPosAry(,) As Integer, ByVal V_EndPosAry(,) As Integer) As Integer

    '        Dim r As Integer
    '        Dim strMSG As String

    '        Try
    '            ' ���[���a�d�l�łȂ����NOP
    '            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then
    '                Return (cFRS_NORMAL)
    '            End If
    '            ' ����p�I�u�W�F�N�g�������Ȃ�NOP
    '            If (ObjPrint Is Nothing) Then
    '                Return (cFRS_NORMAL)
    '            End If

    '            ' �������
    '            r = ObjPrint.Set_Keisen_Pos(KWidth, H_StaPosAry, H_EndPosAry, V_StaPosAry, V_EndPosAry)
    '            Return (r)

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "QR_Print.Print_Data() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '            Return (cERR_TRAP)
    '        End Try
    '    End Function
    '#End Region

    '#Region "�g���~���O���ʌ��ʈ������"
    '    '''=========================================================================
    '    ''' <summary>�g���~���O���ʌ��ʈ������</summary>
    '    ''' <param name="Md">(INP)0=�ʏ���, 1=Print�{�^������</param>
    '    ''' <remarks></remarks>
    '    '''=========================================================================
    '    Public Sub PrnTrimResult(ByVal Md As Integer)

    '        Dim r As Integer
    '        Dim Idx As Integer
    '        Dim sLINE_TITLE(ITEM_MAX) As String                             ' ���o������
    '        Dim sLINE_DATA(ITEM_MAX) As String                              ' ����f�[�^
    '        Dim strHEAD As String
    '        Dim strDAT As String
    '        Dim strMSG As String = ""

    '        Try
    '            '-------------------------------------------------------------------
    '            '   �����ݒ�
    '            '-------------------------------------------------------------------
    '            ' ���[���a�����łȂ����NOP
    '            If (gSysPrm.stCTM.giSPECIAL <> customROHM) Then Return

    '            ' �f�[�^�����[�h�Ȃ��NOP
    '            If (gLoadDTFlag = False) Then Return

    '            ' �蓮�^�]�Ȃ�NOP
    '            If (stPRT_ROHM.bAutoMode = False) Then Return

    '            ' ����p�̍�Ɠ����Z�b�g����Ă��Ȃ����NOP
    '            If (stPRT_ROHM.DateR = "") Then Return

    '            ' �uPrint ON/OFF�v�{�^�����uPrint OFF�v�Ȃ�NOP
    '            If (gSysPrm.stDEV.rPrnOut_flg = False) Then Return

    '            ' �g���~���O���ʈ�����ڂ̃f�[�^���Z�b�g����(����O�ɐݒ�\�Ȃ���)
    '            Call SetTrimPrnData()

    '            '-------------------------------------------------------------------
    '            '   �r������p�f�[�^��ݒ肷��
    '            '-------------------------------------------------------------------
    '            ' �����C���J�n���WX,Y�z��X(10, 10, .... 10, 10),Y(10, 15, ...., 120)
    '            H_StaPosAry = New Integer(1, H_LINE_NUM) _
    '            {{PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, PR_H0_POSX, _
    '              PR_H0_POSX, PR_H0_POSX}, _
    '             {PR_H00_POSY, PR_H01_POSY, PR_H02_POSY, PR_H03_POSY, PR_H04_POSY, PR_H05_POSY, PR_H06_POSY, PR_H07_POSY, PR_H08_POSY, PR_H09_POSY, _
    '              PR_H10_POSY, PR_H11_POSY, PR_H12_POSY, PR_H13_POSY, PR_H14_POSY, PR_H15_POSY, PR_H16_POSY, PR_H17_POSY, PR_H18_POSY, PR_H19_POSY, _
    '              PR_H20_POSY, PR_H21_POSY, PR_H22_POSY, PR_H23_POSY, PR_H24_POSY, PR_H25_POSY, PR_H26_POSY, PR_H27_POSY, PR_H28_POSY, PR_H29_POSY, _
    '              PR_H30_POSY, PR_H31_POSY}}

    '            ' �����C���I�����WX,Y�z��X(110, 110, .... 110, 110),Y(10, 15, ...., 120)
    '            H_EndPosAry = New Integer(1, H_LINE_NUM) _
    '            {{PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, PR_V3_POSX, _
    '              PR_V3_POSX, PR_V3_POSX}, _
    '             {PR_H00_POSY, PR_H01_POSY, PR_H02_POSY, PR_H03_POSY, PR_H04_POSY, PR_H05_POSY, PR_H06_POSY, PR_H07_POSY, PR_H08_POSY, PR_H09_POSY, _
    '              PR_H10_POSY, PR_H11_POSY, PR_H12_POSY, PR_H13_POSY, PR_H14_POSY, PR_H15_POSY, PR_H16_POSY, PR_H17_POSY, PR_H18_POSY, PR_H19_POSY, _
    '              PR_H20_POSY, PR_H21_POSY, PR_H22_POSY, PR_H23_POSY, PR_H24_POSY, PR_H25_POSY, PR_H26_POSY, PR_H27_POSY, PR_H28_POSY, PR_H29_POSY, _
    '              PR_H30_POSY, PR_H31_POSY}}

    '            ' �c���C���J�n���WX(10, 20, 65, 110),Y(10, ....., 10)�z��
    '            V_StaPosAry = New Integer(1, V_LINE_NUM) _
    '            {{PR_V0_POSX, PR_V1_POSX, PR_V2_POSX, PR_V3_POSX}, _
    '             {PR_V0_POSY, PR_V0_POSY, PR_V0_POSY, PR_V0_POSY}}

    '            ' �c���C���I�����WX(10, 20, 65, 110),Y(165, ....., 165)�z��
    '            V_EndPosAry = New Integer(1, V_LINE_NUM) _
    '            {{PR_V0_POSX, PR_V1_POSX, PR_V2_POSX, PR_V3_POSX}, _
    '             {PR_H31_POSY, PR_H31_POSY, PR_H31_POSY, PR_H31_POSY}}

    '            ' �r���ʒu��ݒ肷��
    '            r = Set_Keisen_Pos(KWidth, H_StaPosAry, H_EndPosAry, V_StaPosAry, V_EndPosAry)

    '            '-------------------------------------------------------------------
    '            '   ����p���o�����ڂ�ݒ肷��
    '            '-------------------------------------------------------------------
    '            Idx = 1
    '            sLINE_TITLE(Idx) = "" : Idx = Idx + 1                       '  1.��Ɠ�
    '            sLINE_TITLE(Idx) = "START TIME" : Idx = Idx + 1             '  2.�J�n����
    '            sLINE_TITLE(Idx) = "STOP TIME" : Idx = Idx + 1              '  3.�I������
    '            sLINE_TITLE(Idx) = "PROG TIME" : Idx = Idx + 1              '  4.�J�n�`�I���܂łɗv��������
    '            sLINE_TITLE(Idx) = "OPE TIME" : Idx = Idx + 1               '  5.�ғ�����
    '            sLINE_TITLE(Idx) = "ALARM TIME" : Idx = Idx + 1             '  6.�װтɂ���~��������
    '            sLINE_TITLE(Idx) = "OPE RATE" : Idx = Idx + 1               '  7.�ғ���
    '            sLINE_TITLE(Idx) = "MTBF" : Idx = Idx + 1                   '  8.���ό̏�Ԋu
    '            sLINE_TITLE(Idx) = "MTTR" : Idx = Idx + 1                   '  9.���ϕ�������
    '            sLINE_TITLE(Idx) = "LOT NO" : Idx = Idx + 1                 ' 10.QR�R�[�h���̃��b�g�ԍ�
    '            sLINE_TITLE(Idx) = "Qrate" : Idx = Idx + 1                  ' 11.���ݸ�Qڰ�
    '            sLINE_TITLE(Idx) = "TrIdxm Speed" : Idx = Idx + 1           ' 12.���ݸ޶�Ľ�߰��
    '            sLINE_TITLE(Idx) = "Trim OK" : Idx = Idx + 1                ' 13.�Ǖi���ߐ�
    '            sLINE_TITLE(Idx) = "Pretest Lo Fail" : Idx = Idx + 1        ' 14.�����l�����s�ǂ����ߐ�
    '            sLINE_TITLE(Idx) = "Pretest Hi Fail" : Idx = Idx + 1        ' 15.�����l����s�ǂ����ߐ�
    '            sLINE_TITLE(Idx) = "Pretest Open" : Idx = Idx + 1           ' 16.�����l����ݕs�ǂ����ߐ�
    '            'sLINE_TITLE(Idx) = "Cut NG": Idx = Idx + 1                 '   .���ݸގ��ɖڕW�l�ɒB���Ȃ��������ߐ�
    '            sLINE_TITLE(Idx) = "Pretest NG" : Idx = Idx + 1             ' 17.�����s��
    '            sLINE_TITLE(Idx) = "Final test Lo Fail" : Idx = Idx + 1     ' 18.���ݸތ�̉����s�ǂ����ߐ�
    '            sLINE_TITLE(Idx) = "Final test Hi Fail" : Idx = Idx + 1     ' 19.���ݸތ�̏���s�ǂ����ߐ�
    '            sLINE_TITLE(Idx) = "Final test Open" : Idx = Idx + 1        ' 20.���ݸތ�ɵ���ݴװ�ƂȂ������ߐ�
    '            sLINE_TITLE(Idx) = "Yield" : Idx = Idx + 1                  ' 21.�Ǖi���ߐ������ߐ�
    '            sLINE_TITLE(Idx) = "Yield(%)" : Idx = Idx + 1               ' 22.��L��%�\��
    '            sLINE_TITLE(Idx) = "Pdt Sheet" : Idx = Idx + 1              ' 23.���ݸ޽ð�ނŏ������������
    '            sLINE_TITLE(Idx) = "Lot Sheet" : Idx = Idx + 1              ' 24.���u�ɓ������ꂽۯĖ���
    '            sLINE_TITLE(Idx) = "Lot NG Sheet" : Idx = Idx + 1           ' 25.ۯĒ��̕s�Ǌ��
    '            sLINE_TITLE(Idx) = "Edg Fail" : Idx = Idx + 1               ' 26.ۯĒ��̔F���s�Ǌ����
    '            sLINE_TITLE(Idx) = "Nominal" : Idx = Idx + 1                ' 27.�ڕW��R�l
    '            'sLINE_TITLE(Idx) = "Trim Target" : Idx = Idx + 1            ' �␳��̖ڕW��R�l
    '            'sLINE_TITLE(Idx) = "Trim Limit" : Idx = Idx + 1             ' ���ݸޖڕW�␳�l
    '            sLINE_TITLE(Idx) = "Mean Value" : Idx = Idx + 1             ' 28.���ݸނ��ꂽ���߂̕��ϒ�R�l
    '            sLINE_TITLE(Idx) = "%" : Idx = Idx + 1                      ' 29.��L��%�\��
    '            sLINE_TITLE(Idx) = "M/R" : Idx = Idx + 1                    ' 30.���ϒl�̌덷
    '            sLINE_TITLE(Idx) = "S/_x" : Idx = Idx + 1                   ' 31.���ݸނ��ꂽ���߂̌덷�̕W���΍�

    '            '-------------------------------------------------------------------
    '            '   ����f�[�^�p��ݒ肷��
    '            '-------------------------------------------------------------------
    '            Idx = 1
    '            With stPRT_ROHM
    '                sLINE_DATA(Idx) = .DateR : Idx = Idx + 1                                ' 1.��Ɠ�
    '                sLINE_DATA(Idx) = .START_TIME : Idx = Idx + 1                           ' 2.�J�n����
    '                sLINE_DATA(Idx) = .STOP_TIME : Idx = Idx + 1                            ' 3.�I������
    '                sLINE_DATA(Idx) = .PROG_TIME : Idx = Idx + 1                            ' 4.�J�n�`�I���܂łɗv��������
    '                sLINE_DATA(Idx) = .OPE_TIME : Idx = Idx + 1                             ' 5.�ғ�����
    '                sLINE_DATA(Idx) = .ALARM_TIME : Idx = Idx + 1                           ' 6.�װтɂ���~��������
    '                sLINE_DATA(Idx) = .OPE_RATE : Idx = Idx + 1                             ' 7.�ғ���
    '                sLINE_DATA(Idx) = .MTBF : Idx = Idx + 1                                 ' 8.���ό̏�Ԋu
    '                sLINE_DATA(Idx) = .MTTR : Idx = Idx + 1                                 ' 9.���ϕ�������
    '                sLINE_DATA(Idx) = .LOT_NO : Idx = Idx + 1                               '10.QR�R�[�h���̃��b�g�ԍ�
    '                sLINE_DATA(Idx) = .Qrate : Idx = Idx + 1                                '11.���ݸ�Qڰ�
    '                sLINE_DATA(Idx) = .Trim_Speed : Idx = Idx + 1                           '12.���ݸ޶�Ľ�߰��
    '                sLINE_DATA(Idx) = .Trim_OK.ToString("0") : Idx = Idx + 1                '13.�Ǖi���ߐ�
    '                sLINE_DATA(Idx) = .Pretest_Lo_Fail.ToString("0") : Idx = Idx + 1        '14.�����l�����s�ǂ����ߐ�
    '                sLINE_DATA(Idx) = .Pretest_Hi_Fail.ToString("0") : Idx = Idx + 1        '15.�����l����s�ǂ����ߐ�
    '                sLINE_DATA(Idx) = .Pretest_Open.ToString("0") : Idx = Idx + 1           '16.�����l����ݕs�ǂ����ߐ�
    '                'sLINE_DATA(Idx) = .Cut_NG.ToString("0"): Idx = Idx + 1                 ' ���ݸގ��ɖڕW�l�ɒB���Ȃ��������ߐ�
    '                sLINE_DATA(Idx) = .Pretest_NG_Cut_NG.ToString("0") : Idx = Idx + 1      '17.�����s��
    '                sLINE_DATA(Idx) = .Final_test_Lo_Fail.ToString("0") : Idx = Idx + 1     '18.���ݸތ�̉����s�ǂ����ߐ�
    '                sLINE_DATA(Idx) = .Final_test_Hi_Fail.ToString("0") : Idx = Idx + 1     '19.���ݸތ�̏���s�ǂ����ߐ�
    '                sLINE_DATA(Idx) = .Final_test_Open.ToString("0") : Idx = Idx + 1        '20.���ݸތ�ɵ���ݴװ�ƂȂ������ߐ�
    '                sLINE_DATA(Idx) = .Yield : Idx = Idx + 1                                '21.�Ǖi���ߐ������ߐ�
    '                sLINE_DATA(Idx) = .Yield_Par.ToString("0.000") & "%" : Idx = Idx + 1    '22.��L��%�\��
    '                sLINE_DATA(Idx) = .Pdt_Sheet.ToString("0") : Idx = Idx + 1              '23.���ݸ޽ð�ނŏ������������
    '                sLINE_DATA(Idx) = .Lot_Sheet.ToString("0") : Idx = Idx + 1              '24.���u�ɓ������ꂽۯĖ���
    '                sLINE_DATA(Idx) = .Lot_NG_Sheet.ToString("0") : Idx = Idx + 1           '25.ۯĒ��̕s�Ǌ��
    '                sLINE_DATA(Idx) = .Edg_Fail.ToString("0") : Idx = Idx + 1               '26.ۯĒ��̔F���s�Ǌ����
    '                sLINE_DATA(Idx) = .Nominal.ToString("0.00000") : Idx = Idx + 1          '27.�ڕW��R�l
    '                'sLINE_DATA(Idx) = .Trim_Target.ToString("0.00000") : Idx = Idx + 1      ' �␳��̖ڕW��R�l
    '                'sLINE_DATA(Idx) = .Trim_Limit.ToString("0.00") : Idx = Idx + 1          ' ���ݸޖڕW�␳�l
    '                sLINE_DATA(Idx) = .Mean_Value.ToString("0.00000") : Idx = Idx + 1       '28.���ݸނ��ꂽ���߂̕��ϒ�R�l
    '                sLINE_DATA(Idx) = ._Par.ToString("0.000") & "%" : Idx = Idx + 1         '29.��L��%�\��
    '                sLINE_DATA(Idx) = .M_R.ToString("0.000") & "%" : Idx = Idx + 1          '30.���ϒl�̌덷
    '                sLINE_DATA(Idx) = .Prn3s_x.ToString("0.000") & "%" : Idx = Idx + 1      '31.���ݸނ��ꂽ���߂̌덷�̕W���΍�
    '            End With

    '            '-------------------------------------------------------------------
    '            '   ����p�e�L�X�g�{�b�N�X(TxtBoxPrint(��\��))�ɏo�͂���
    '            '-------------------------------------------------------------------
    '            ' ���o���ݒ� ("Date/Time : " & Date & "  " & Time + �t�@�C����)
    '            Sub_GetFileName(gStrTrimFileName, strMSG)                   ' �t�@�C�����݂̂����o�� 
    '            strHEAD = "Date/Time : " & Today.ToString("yyyy/MM/dd") & "  " & TimeOfDay.ToString("HH:mm:ss") & "    " & strMSG

    '            ' ����p�e�L�X�g�{�b�N�X�ɏo��
    '            For Idx = 1 To ITEM_MAX                                     ' �ő區�ڐ����J��Ԃ�
    '                ' No.
    '                strMSG = Idx.ToString("0")
    '                strDAT = strMSG.PadLeft(4) + "  "                       ' No.    (4���� �����ɋ󔒃p�f�B���O)
    '                sLINE_TITLE(Idx) = sLINE_TITLE(Idx).PadRight(25)        ' ���o��(25���� �E���ɋ󔒃p�f�B���O)
    '                sLINE_DATA(Idx) = sLINE_DATA(Idx).PadRight(30)          ' ���ځ@(30���� �E���ɋ󔒃p�f�B���O)
    '                ' No.x + ���o�� + ����
    '                strDAT = strDAT & sLINE_TITLE(Idx) & "  " & sLINE_DATA(Idx) & vbCrLf
    '                ' ����p�e�L�X�g�{�b�N�X�ɏo��
    '                If (Idx = 1) Then
    '                    Form1.TxtBoxPrint.Text = strDAT
    '                Else
    '                    Form1.TxtBoxPrint.AppendText(strDAT)
    '                End If
    '            Next Idx

    '            '-------------------------------------------------------------------
    '            '   �������(�e�L�X�g�{�b�N�X(TxtBoxPrint)�̕�������������)
    '            '-------------------------------------------------------------------
    '            r = Print_Data(Form1.TxtBoxPrint.Text, strHEAD, HEAD_POSX, HEAD_POSY)
    '            If (r <> cFRS_NORMAL) Then
    '                ' "��������Ɏ��s���܂����B(r=xxxx)"
    '                strMSG = MSG_153 + "(r = " + r.ToString("0") + ")"
    '                Call MsgBox(strMSG, vbOKOnly)
    '            End If

    '            ' �g���b�v�G���[������ 
    '        Catch ex As Exception
    '            strMSG = "QR_Print.ClrTrimPrnData() TRAP ERROR = " + ex.Message
    '            MsgBox(strMSG)
    '        End Try
    '    End Sub
    '#End Region
#End Region

#End Region
End Module
