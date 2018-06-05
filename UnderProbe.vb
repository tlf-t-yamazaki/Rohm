'===============================================================================
'   Description  : �����v���[�u�p�Ƀt�@�C����ǉ� V1.13.0.1�@
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@

Module UnderProbe

#Region "���\�b�h�̒�`"

#Region "�����v���[�u�L���`�F�b�N"
    ''=========================================================================
    '' <summary>�����v���[�u�̗L���m�F</summary>
    '' <returns>�L��: True ����: False</returns>
    '' <remarks>�����v���[�u�̗L���m�F</remarks>
    ''=========================================================================
    Public Function IsUnderProbe() As Boolean

        If (gSysPrm.stDEV.giPrbTyp And 2) = 0 Then                  ' Z2(����)�Ȃ��H
            IsUnderProbe = False
        Else
            IsUnderProbe = True
        End If

    End Function
#End Region

#Region "����Z2���̓���"
    ''=========================================================================
    '' <summary>����Z2�v���[�u��ON/OFF���s��</summary>
    '' <param name="offon">(INP)ON/OFF�w��</param>
    '' <returns>cFRS_NORMAL   = ����
    ''          ��L�ȊO      = ����~�����̑��G���[
    '' </returns>
    ''=========================================================================
    Public Function Z2move(ByVal offon As Integer) As Integer

        Dim z As Double
        Dim r As Short
        Dim strMSG As String

        Try

            If offon = Z2ON Then
                z = typPlateInfo.dblLwPrbStpUpDist                          ' �v���[�u�n�m���t�o
            ElseIf offon = Z2STEP Then
                ' Z = �����v���[�u�X�e�b�v�㏸�����|�����v���[�u�X�e�b�v���~����=�X�e�b�v�㏸����
                'z = typPlateInfo.dblLwPrbStpUpDist - typPlateInfo.dblLwPrbStpDwDist
                z = 0.0
                If z >= typPlateInfo.dblLwPrbStpUpDist Then                 ' �s��l�Ȃ牺�܂ŉ�����B
                    z = 0
                End If
            Else
                z = 0                                                       ' �v���[�u�n�e�e���c�n�v�m
            End If

            ' Z2�ړ�
            r = ZZMOVE2(z, 1)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)               ' �G���[�Ȃ烁�b�Z�[�W�\������   
            Return (r)

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "UnderProbe.Z2move() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                              ' �ߒl = �g���b�v�G���[
        End Try
    End Function
#End Region

#End Region

End Module
