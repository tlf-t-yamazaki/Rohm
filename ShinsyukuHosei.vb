'===============================================================================
'   Description  : �L�k�␳�p�Ƀt�@�C����ǉ� 'V1.13.0.0X
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'===============================================================================

Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@

Module ShinsyukuHosei

    Const UNDER_CAMERA As Integer = 1               ' �����J�����ԍ�


    Private Const ShinsyukuPath As String = "C:\TRIM\Tky.ini"

    Public Structure POINTD
        Dim x As Double
        Dim y As Double
    End Structure



#Region "���\�b�h�̒�`"

    Public Function ShinsyukuHoseiMain() As Integer

        Dim X As Integer
        Dim Y As Integer
        Dim Rtn As Integer
        Dim shiftx As Double
        Dim shifty As Double
        Dim Thresh As Double
        Dim stageOffsetX As Double
        Dim stageOffsetY As Double
        Dim strLOG As String
        Dim strMSG As String
        Dim StgX As Double = 0.0 ' V4.0.0.0-40
        Dim StgY As Double = 0.0 ' V4.0.0.0-40

        Try
            ' �␳�L���`�F�b�N
            If typPlateInfo.intContExpMode = 0 Then
                Return (cFRS_NORMAL)
            End If

            ' �u���b�N���_�ݒ�ݒ�
            SetOriginalBlockCoord()

            ' �L�k�␳�ʒu�ւ̈ړ��Ɖ摜�F�����s
            For X = 0 To typPlateInfo.intBlockCntXDir - 1
                For Y = 0 To typPlateInfo.intBlockCntYDir - 1
                    If SelectBlock(X, Y) = 1 Then
                        ' �I������Ă�����A�ʒu�ړ��Ɖ摜�F�����s
                        ' XY�e�[�u�����u���b�N���_�ֈړ�����
                        If (typPlateInfo.intReviseMode = 1) Then
                            stageOffsetX = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + typPlateInfo.dblContExpPosX
                            stageOffsetY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + typPlateInfo.dblContExpPosY
                        Else
                            stageOffsetX = typPlateInfo.dblTableOffsetXDir + gfCorrectPosX + (typPlateInfo.dblBlockSizeXDir / 2) + typPlateInfo.dblContExpPosX
                            stageOffsetY = typPlateInfo.dblTableOffsetYDir + gfCorrectPosY + (typPlateInfo.dblBlockSizeYDir / 2) + typPlateInfo.dblContExpPosY
                        End If
                        '----- V2.0.0.0�H�� -----
                        If (giMachineKd = MACHINE_KD_RS) Then
                            '----- V4.0.0.0-40�� -----
                            ' SL36S���ŃX�e�[�WY�̌��_�ʒu����̏ꍇ�A�u���b�N�T�C�Y��1/2�͉��Z���Ȃ�
                            If (giStageYOrg = STGY_ORG_UP) Then
                                StgX = OriginalBlock(X, Y).x + stageOffsetX
                                StgY = OriginalBlock(X, Y).y + stageOffsetY
                            Else
                                StgX = OriginalBlock(X, Y).x + stageOffsetX
                                StgY = OriginalBlock(X, Y).y + stageOffsetY + (typPlateInfo.dblBlockSizeYDir / 2)
                            End If
                            Rtn = Form1.System1.EX_START(gSysPrm, StgX, StgY, 0)

                            'Rtn = Form1.System1.EX_START(gSysPrm, OriginalBlock(X, Y).x + stageOffsetX, OriginalBlock(X, Y).y + stageOffsetY + (typPlateInfo.dblBlockSizeYDir / 2), 0)
                            '----- V4.0.0.0-40�� ----
                        Else
                            Rtn = Form1.System1.EX_START(gSysPrm, OriginalBlock(X, Y).x + stageOffsetX, OriginalBlock(X, Y).y + stageOffsetY, 0)
                        End If
                        'Rtn = Form1.System1.EX_START(gSysPrm, OriginalBlock(X, Y).x + stageOffsetX, OriginalBlock(X, Y).y + stageOffsetY, 0)
                        '----- V2.0.0.0�H�� -----
                        '                    Rtn = Form1.System1.EX_SMOVE2(gSysPrm, OriginalBlock(X, Y).x + typPlateInfo.dblContExpPosX, OriginalBlock(X, Y).y + typPlateInfo.dblContExpPosY)
                        If (Rtn <> cFRS_NORMAL) Then                                  ' �G���[ ? 
                            Return (Rtn)
                        End If

                        ' �摜�F�����s
                        Rtn = Form1.VideoLibrary1.PtnMatchingSelect(UNDER_CAMERA, typPlateInfo.intContExpPtnNo, Thresh, shiftx, shifty)
                        If Rtn = 1 Then     ' ����ʂ����̃u���b�N���_�ɑ���
                            'OriginalBlock(X, Y).x = Coordinates(X, Y).x + shiftx
                            'OriginalBlock(X, Y).y = Coordinates(X, Y).y + shifty
                            Coordinates(X, Y).x = shiftx
                            Coordinates(X, Y).y = shifty
                            strLOG = "BlockNo X = " & X + 1 & ", BlockNo Y = " & Y + 1 & " shift x = " & shiftx.ToString("0.0000") & ":" & "shift y = " & shifty.ToString("0.0000")
                            Call Form1.Z_PRINT(strLOG)
                        Else
                            Return (Rtn)
                        End If
                    End If
                Next Y
            Next X

            ' ���ԕ␳�l�v�Z
            Rtn = CalcCoordinate(typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir)
            Rtn = CheckThresholds(typPlateInfo.intBlockCntXDir, typPlateInfo.intBlockCntYDir)
            Return (Rtn)
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function

    Public Function CalcCoordinate(ByVal BlockCountX As Integer, ByVal BlockCountY As Integer) As Integer
        Dim X As Integer
        Dim Y As Integer
        Dim posX As Double
        Dim posY As Double
        Dim CalcPosX As Integer
        Dim CalcPosY As Integer
        Dim BaseBlockX As Integer
        Dim BaseBlockY As Integer
        Dim startPoint As Integer
        Dim nextPoint As Integer
        Dim endPoint As Integer
        Dim strMSG As String

        Try
            ' ���ӂ̕␳�l�����܂��Ă��Ȃ��ƁA�����̌v�Z�ł��Ȃ����߁A��Ɏ��͂̌v�Z�����s����
            BaseBlockX = 0
            BaseBlockY = 0
            If BlockCountX = 1 Or BlockCountY = 1 Then
                Exit Function
            End If
            ' ��ԏ�ƈ�ԉ��̕␳�l���v�Z
            For Y = 0 To (BlockCountY - 1) Step (BlockCountY - 1)
                For X = 0 To (BlockCountX - 1) Step (BlockCountX - 1)
                    If SelectBlock(X, Y) = 1 Then
                        startPoint = X
                        For nextPoint = startPoint + 1 To (BlockCountX - 1)
                            If (SelectBlock(nextPoint, Y) = 1) Or (nextPoint = (BlockCountX - 1)) Then
                                endPoint = nextPoint
                                For CalcPosX = startPoint To endPoint
                                    Call GetHoseiValueX(startPoint, endPoint, CalcPosX, Y, posX, posY)          ' 2�_�Ԃ̕␳�l����A�w��_�ł̕␳�l���v�Z����
                                    Coordinates(CalcPosX, Y).x = posX
                                    Coordinates(CalcPosX, Y).y = posY
                                Next CalcPosX
                                Exit For
                            End If
                        Next nextPoint
                    End If
                    BaseBlockX = X + 1
                Next X
                BaseBlockX = 0
            Next Y

            ' ��ԍ��ƈ�ԉE�̕␳�l���v�Z
            BaseBlockX = 0
            BaseBlockY = 0
            For X = 0 To (BlockCountX - 1) Step (BlockCountX - 1)
                For Y = 0 To (BlockCountY - 1) Step (BlockCountY - 1)
                    If SelectBlock(X, Y) = 1 Then
                        startPoint = Y
                        For nextPoint = startPoint + 1 To (BlockCountY - 1)
                            If ((SelectBlock(X, nextPoint) = 1) Or (nextPoint = (BlockCountY - 1))) Then
                                endPoint = nextPoint
                                For CalcPosY = startPoint To endPoint
                                    Call GetHoseiValueY(startPoint, endPoint, CalcPosY, X, posX, posY)          ' 2�_�Ԃ̕␳�l����A�w��_�ł̕␳�l���v�Z����
                                    Coordinates(X, CalcPosY).x = posX
                                    Coordinates(X, CalcPosY).y = posY
                                Next CalcPosY
                            End If

                        Next nextPoint
                    End If
                    BaseBlockY = Y + 1
                Next Y
                BaseBlockY = 0
            Next X


            ' ���Ԓn�_�ł̕␳�l���v�Z����B
            BaseBlockX = 0
            BaseBlockY = 0
            For Y = 0 To (BlockCountY - 1)
                startPoint = 0
                endPoint = BlockCountX - 1
                For X = 0 To (BlockCountX - 1)
                    If (SelectBlock(X, Y) = 1) Or (X = 0) Then
                        startPoint = X
                        For nextPoint = startPoint + 1 To (BlockCountX - 1)
                            If (SelectBlock(nextPoint, Y) = 1) Or (nextPoint = (BlockCountX - 1)) Then
                                endPoint = nextPoint
                            End If
                        Next nextPoint
                        ' �Y������`�F�b�N���Ȃ������ꍇ�ɂ͗��[�̒l��␳�l�Ƃ��Čv�Z����
                        For CalcPosX = startPoint To endPoint
                            Call GetHoseiValueX(startPoint, endPoint, CalcPosX, Y, posX, posY)          ' 2�_�Ԃ̕␳�l����A�w��_�ł̕␳�l���v�Z����
                            Coordinates(CalcPosX, Y).x = posX
                            Coordinates(CalcPosX, Y).y = posY
                        Next CalcPosX
                        BaseBlockX = X
                    End If
                Next X
                BaseBlockX = 0
            Next Y

            CalcCoordinate = cFRS_NORMAL
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function

    ' 2013.11.09
    ' �Q�_�Ԃ�X�����̋ߎ��ʒu�̎擾
    ' �␳�����ʒu���m�̕␳�l���ϓ����肵�āA���ݒl�ɉ��Z����.
    ' OriginalPos()�ɂ̓v���[�g�f�[�^�ł̌v�Z��̃u���b�N���_�������Ă���
    ' Coordinates()�ɂ́A�␳�ʂ������Ă���
    ' ���Ƃ�O��Ƃ��Čv�Z����B
    Public Sub GetHoseiValueX(ByVal StartPoint As Integer, ByVal EndPoint As Integer, ByVal PointN As Integer, ByVal PointY As Integer, ByRef posX As Double, ByRef POSY As Double)
        Dim strMSG As String
        Try
            If (EndPoint <> StartPoint) Then
                posX = ((Coordinates(EndPoint, PointY).x - Coordinates(StartPoint, PointY).x) / (EndPoint - StartPoint)) * PointN + Coordinates(StartPoint, PointY).x
                POSY = ((Coordinates(EndPoint, PointY).y - Coordinates(StartPoint, PointY).y) / (EndPoint - StartPoint)) * PointN + Coordinates(StartPoint, PointY).y
            Else
                posX = Coordinates(EndPoint, PointY).x '����ʂ�����ɕύX + OriginalBlock(StartPoint, PointY).x
                POSY = Coordinates(EndPoint, PointY).y '����ʂ�����ɕύX + OriginalBlock(StartPoint, PointY).x
            End If
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub

    ' 2013.11.09
    ' �Q�_�Ԃ�Y�����̋ߎ��ʒu�̎擾
    ' �␳�l���ϓ����肵�āA���ݒl�ɉ��Z����.
    ' OriginalPos()�ɂ̓v���[�g�f�[�^�ł̌v�Z��̃u���b�N���_�������Ă���
    ' Coordinates()�ɂ́A�␳�ʂ������Ă���
    ' ���Ƃ�O��Ƃ��Čv�Z����B
    Public Sub GetHoseiValueY(ByVal StartPoint As Integer, ByVal EndPoint As Integer, ByVal PointN As Integer, ByVal PointX As Integer, ByRef posX As Double, ByRef posy As Double)
        Dim strMSG As String
        Try
            If (EndPoint <> StartPoint) Then
                posX = ((Coordinates(PointX, EndPoint).x - Coordinates(PointX, StartPoint).x) / (EndPoint - StartPoint)) * PointN + Coordinates(PointX, StartPoint).x
                posy = ((Coordinates(PointX, EndPoint).y - Coordinates(PointX, StartPoint).y) / (EndPoint - StartPoint)) * PointN + Coordinates(PointX, StartPoint).y
            Else
                posX = Coordinates(PointX, EndPoint).x ' + OriginalBlock(PointX, StartPoint).y
                posy = Coordinates(PointX, EndPoint).y ' + OriginalBlock(PointX, StartPoint).y
            End If
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetHoseiValueY() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Sub

    ' �u���b�N���_�ݒ�ݒ�
    Private Sub SetOriginalBlockCoord()
        Dim r As Integer
        Dim x As Integer
        Dim y As Integer
        Dim stgx As Double
        Dim stgy As Double
        Dim tempX As Double
        Dim tempY As Double
        Dim strMSG As String

        Try
            For y = 0 To typPlateInfo.intBlockCntYDir - 1
                For x = 0 To typPlateInfo.intBlockCntXDir - 1
                    r = GetTargetStagePosByXY((x + 1), (y + 1), stgx, stgy)
                    Coordinates(x, y).x = 0
                    Coordinates(x, y).y = 0
                    OriginalBlock(x, y).x = stgx
                    OriginalBlock(x, y).y = stgy
                    If x = 0 And y = 0 Then
                        tempX = OriginalBlock(0, 0).x
                        tempY = OriginalBlock(0, 0).y
                    End If
                    OriginalBlock(x, y).x = OriginalBlock(x, y).x - tempX
                    OriginalBlock(x, y).y = OriginalBlock(x, y).y - tempY
                Next x
            Next y
        Catch ex As Exception
            strMSG = "shinsyukuhosei - SetOriginalBlockCoord() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#Region "LOAD�ESAVE�̋��ʉ��ɂ��TrimDataEditor�œ�������"
#If False Then 'V5.0.0.8�@
    ' �u���b�N���_�I����Ԃ̃N���A
    Public Sub ClearBlockSelect()
        Dim x As Integer
        Dim y As Integer
        Dim strMSG As String
        Try
            For y = 0 To BLOCK_COUNT_MAX - 1
                For x = 0 To BLOCK_COUNT_MAX - 1
                    SelectBlock(x, y) = 0
                Next x
            Next y
        Catch ex As Exception
            strMSG = "shinsyukuhosei - ClearBlockSelect() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End If
#End Region 'V5.0.0.8�@
    '''=========================================================================
    '''<summary>�L�k�␳�␳�ʂ��擾����</summary>
    '''<param name="BlkX"> (INP)X�����u���b�N�ԍ�)</param>
    '''<param name="BlkY"> (INP)Y�����u���b�N�ԍ�</param>
    '''<param name="posx"> (OUT)X�����I�t�Z�b�g</param>
    '''<param name="posy"> (OUT)Y�����I�t�Z�b�g</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================
    Public Function GetShinsyukuData(ByVal BlkX As Integer, ByVal BlkY As Integer, ByRef posx As Double, ByRef posy As Double) As Integer
        Dim strMSG As String

        Try
            ' �␳�L���`�F�b�N
            If typPlateInfo.intContExpMode = 0 Then
                Return (cFRS_NORMAL)
            End If
            posx = posx - Coordinates(BlkX - 1, BlkY - 1).x
            posy = posy + Coordinates(BlkX - 1, BlkY - 1).y
            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetShinsyukuData() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function

    '----------------------------------------------------------------------------
    '  �֐���: CheckThresholds
    '  �@�\�@: �p�^�[���F���S�_�␳�v�Z臒l�`�F�b�N
    '  �p�����[�^�F BlockX,BlockY IN: �w�C�x�����̃u���b�N��
    '----------------------------------------------------------------------------
    '''=========================================================================
    '''<summary>�p�^�[���F���S�_�␳�v�Z臒l�`�F�b�N</summary>
    '''<param name="blockx"> (INP)X�����u���b�N��)</param>
    '''<param name="blocky"> (INP)Y�����u���b�N��</param>
    '''<returns>0=����, 0�ȊO=�G���[</returns>
    '''=========================================================================

    Public Function CheckThresholds( _
        ByVal blockx As Long, ByVal blocky As Long _
    ) As Integer

        Dim LengthOrg(4) As Double
        Dim Length(4) As Double
        Dim Threshold As Double
        Dim lstrValue(255) As String
        Dim GetIniString(255) As String
        Dim sMsg As String
        Dim LocalCoordinates(5) As POINTD
        Dim OriginalPos(5) As POINTD
        Dim strMSG As String
        Try
            '            Threshold = Val(Form1.Utility1.GetProfileString_S("CORRECT", "THRESHOLDS", "C:\TRIM\\TRIMMERUSER.INI", "100.0"))
            Threshold = Val(LaserFront.Trimmer.DefWin32Fnc.GetPrivateProfileString_S("CORRECT", "THRESHOLDS", "C:\TRIM\\TRIMMERUSER.INI", "100.0"))

            CheckThresholds = 0

            OriginalPos(1).x = OriginalBlock(0, 0).x
            OriginalPos(2).x = OriginalBlock(blockx - 1, 0).x
            OriginalPos(3).x = OriginalBlock(blockx - 1, blocky - 1).x
            OriginalPos(4).x = OriginalBlock(0, blocky - 1).x

            OriginalPos(1).y = OriginalBlock(0, 0).y
            OriginalPos(2).y = OriginalBlock(blockx - 1, 0).y
            OriginalPos(3).y = OriginalBlock(blockx - 1, blocky - 1).y
            OriginalPos(4).y = OriginalBlock(0, blocky - 1).y

            LocalCoordinates(1).x = Coordinates(0, 0).x + OriginalBlock(0, 0).x
            LocalCoordinates(2).x = Coordinates(blockx - 1, 0).x + OriginalBlock(blockx - 1, 0).x
            LocalCoordinates(3).x = Coordinates(blockx - 1, blocky - 1).x + OriginalBlock(blockx - 1, blocky - 1).x
            LocalCoordinates(4).x = Coordinates(0, blocky - 1).x + OriginalBlock(0, blocky - 1).x

            LocalCoordinates(1).y = Coordinates(0, 0).y + OriginalBlock(0, 0).y
            LocalCoordinates(2).y = Coordinates(blockx - 1, 0).y + OriginalBlock(blockx - 1, 0).y
            LocalCoordinates(3).y = Coordinates(blockx - 1, blocky - 1).y + OriginalBlock(blockx - 1, blocky - 1).y
            LocalCoordinates(4).y = Coordinates(0, blocky - 1).y + OriginalBlock(0, blocky - 1).y

            LengthOrg(1) = GetLength(OriginalPos(2).x, OriginalPos(2).y, OriginalPos(1).x, OriginalPos(1).y)
            LengthOrg(2) = GetLength(OriginalPos(3).x, OriginalPos(3).y, OriginalPos(1).x, OriginalPos(1).y)
            LengthOrg(3) = GetLength(OriginalPos(4).x, OriginalPos(4).y, OriginalPos(3).x, OriginalPos(3).y)
            LengthOrg(4) = GetLength(OriginalPos(4).x, OriginalPos(4).y, OriginalPos(2).x, OriginalPos(2).y)

            Length(1) = GetLength(LocalCoordinates(2).x, LocalCoordinates(2).y, LocalCoordinates(1).x, LocalCoordinates(1).y)
            Length(2) = GetLength(LocalCoordinates(3).x, LocalCoordinates(3).y, LocalCoordinates(1).x, LocalCoordinates(1).y)
            Length(3) = GetLength(LocalCoordinates(4).x, LocalCoordinates(4).y, LocalCoordinates(3).x, LocalCoordinates(3).y)
            Length(4) = GetLength(LocalCoordinates(4).x, LocalCoordinates(4).y, LocalCoordinates(2).x, LocalCoordinates(2).y)

            If Math.Abs(Length(1) - LengthOrg(1)) / (blockx - 1) > Threshold Then
                sMsg = "Threshold Over (side 1)= " & Format((Length(1) - LengthOrg(1)) / (blockx - 1), "0.0000") & " Threshold=" & Format(Threshold, "0.0000")
                Call Form1.Z_PRINT(sMsg)
                CheckThresholds = 101
            End If
            If Math.Abs(Length(2) - LengthOrg(2)) / (blocky - 1) > Threshold Then
                sMsg = "Threshold Over (side 2)= " & Format((Length(2) - LengthOrg(2)) / (blocky - 1), "0.0000") & " Threshold=" & Format(Threshold, "0.0000")
                Call Form1.Z_PRINT(sMsg)
                CheckThresholds = 102
            End If
            If Math.Abs(Length(3) - LengthOrg(3)) / (blockx - 1) > Threshold Then
                sMsg = "Threshold Over (side 3)= " & Format((Length(3) - LengthOrg(3)) / (blockx - 1), "0.0000") & " Threshold=" & Format(Threshold, "0.0000")
                Call Form1.Z_PRINT(sMsg)
                CheckThresholds = 103
            End If
            If Math.Abs(Length(4) - LengthOrg(4)) / (blocky - 1) > Threshold Then
                sMsg = "Threshold Over (side 4)= " & Format((Length(4) - LengthOrg(4)) / (blocky - 1), "0.0000") & " Threshold=" & Format(Threshold, "0.0000")
                Call Form1.Z_PRINT(sMsg)
                CheckThresholds = 104
            End If
        Catch ex As Exception
            strMSG = "shinsyukuhosei - CheckThresholds() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try

    End Function

    '----------------------------------------------------------------------------
    '  �֐���: GetLength
    '  �@�\�@: �������󂯎��2�_�Ԃ̋������v�Z����
    '  �p�����[�^�F x1 = X���W���̂P y1 = Y���W���̂P x2 = X���W����2 y2 = Y���W����2
    '----------------------------------------------------------------------------
    Private Function GetLength(ByVal x1 As Double, ByVal y1 As Double, _
    ByVal x2 As Double, ByVal y2 As Double) _
                              As Double
        Dim strMSG As String
        Try
            GetLength = Math.Sqrt((x2 - x1) ^ 2 + (y2 - y1) ^ 2)

        Catch ex As Exception
            strMSG = "shinsyukuhosei - GetLength() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function


#End Region

End Module
