'==============================================================================
'   Description : �V���v���g���}�p���W���[���t�@�C��
'
'   V2.0.0.0�I
'
'�@ 2014/06/20 First Written by N.Arata(OLFT) 
'
'==============================================================================

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8�@
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Module SimpleTrimmer
#Region "���ʒ�`"

    Friend WithEvents DataClrButton As System.Windows.Forms.Button
    Friend WithEvents DataSaveButton As System.Windows.Forms.Button
    Friend WithEvents CmdBlockData As System.Windows.Forms.Button
    Friend WithEvents CmdLotNumber As System.Windows.Forms.Button
    Friend WithEvents CmdMainte As System.Windows.Forms.Button
    Friend WithEvents CmdProbeClean As System.Windows.Forms.Button
    Friend WithEvents BPAdjustButton As System.Windows.Forms.Button
    Friend WithEvents DataEditButton As System.Windows.Forms.Button     'V4.0.0.0-84

    Friend WithEvents CheckNGRate As System.Windows.Forms.CheckBox      'V4.8.0.1�@


    'V4.0.0.0�L
    Friend WithEvents BlockNextButton As System.Windows.Forms.Button
    Friend WithEvents BlockRvsButton As System.Windows.Forms.Button
    Friend WithEvents BlockNoLabel As System.Windows.Forms.Label
    Friend WithEvents BlockTrimModeLabel As System.Windows.Forms.Label
    'V4.0.0.0�L
    'V4.1.0.0�Q
    Friend WithEvents NowBlockNoLabel As System.Windows.Forms.Label
    'V4.1.0.0�Q

    'V4.0.0.0-76
    Friend WithEvents BlockMainButton As System.Windows.Forms.Button

#If START_KEY_SOFT Then
    Friend WithEvents SoftStartButton As System.Windows.Forms.Button
    Friend WithEvents SoftResetButton As System.Windows.Forms.Button
#End If

    Public TrimData As TrimClassLibrary.TrimData
    Public TrimDef As TrimClassLibrary.TrimDef

    Private Structure StatisticsData
        Dim Label As TKY_ALL_SL432HW.CustomTkyLabel
    End Structure

    ' ���v�f�[�^�̕\���ׂ̈̒�`
    Private Const TKY_BORDER_WIDTH As Integer = 2                               ' ���v�f�[�^�\���r������
    Private Const TKY_TITLE_SIZE As Integer = 100                               ' ���v�f�[�^���o���T�C�Y
    Private Const TKY_DATA_WIDTH As Integer = 480                               ' ���v�f�[�^�S�̕�
    Private Const TKY_DATA_HIGHT As Integer = 32                                ' ���v�f�[�^�c����
    Private Const TKY_DATA_X_OFF As Integer = -1                                ' ���v�f�[�^�������V�t�g���̃I�t�Z�b�g
    Private Const TKY_DATA_Y_OFF As Integer = -1                                ' ���v�f�[�^�������V�t�g���̃I�t�Z�b�g
    Private Const TKY_DATA_INTERVAL As Integer = 10                             ' ���v�f�[�^�u���b�N�ԃC���^�[�o��
    Private TKY_LINE_COLOR As System.Drawing.Color = System.Drawing.Color.Black ' ���v�f�[�^�\���r���F
    Private StatData(83) As StatisticsData                                       ' ���b�g�ԍ��A�J�n���ԁA�o�ߎ��ԁA��
    Private Const TKY_DATA_POS As Integer = 35                                  ' ���v�f�[�^�\���J�n�ʒu�i�T���Z����Ǝ��̍s�j
    Private Const TKY_DATA_COL As Integer = 5                                   ' ���v�f�[�^�\���P�s�̐�

    ' ��R�f�[�^�̕\���ׂ̈̒�`
    Private Const RES_BORDER_WIDTH As Integer = 2                               ' ��R�f�[�^�\���r������
    ''V4.0.0.0�L    Private Const RES_TITLE_SIZE As Integer = 50                                ' ��R�f�[�^���o���T�C�Y
    Private Const RES_TITLE_SIZE As Integer = 40                                ' ��R�f�[�^���o���T�C�Y
    Private Const RES_DATA_HIGHT As Integer = 18                                ' ��R�f�[�^�c����
    Private Const RES_DATA_X_OFF As Integer = -1                                ' ��R�f�[�^�������V�t�g���̃I�t�Z�b�g
    Private Const RES_DATA_Y_OFF As Integer = -1                                ' ��R�f�[�^�������V�t�g���̃I�t�Z�b�g
    Private Const RES_DATA_INTERVAL As Integer = 6                              ' ��R�f�[�^�u���b�N�ԃC���^�[�o��
    Private Const RES_TOTAL_COUNTER As Integer = 50                             ' ��x�ɕ\�������R��
    Private Const RES_DATA_START As Integer = 3                                 ' ��R�f�[�^�\���̐擪�ʒu
    Private Const RES_DATA_STAT As Integer = 153                                ' ���v�f�[�^�\���̐擪�ʒu
    Private RES_LINE_COLOR As System.Drawing.Color = System.Drawing.Color.Black ' ��R�f�[�^�\���r���F
    Private ResistorData(0) As StatisticsData                                   ' ���b�g�ԍ��A�J�n���ԁA�o�ߎ��ԁA��
    Private HeaderButton(2) As System.Windows.Forms.Label                          ' �w�b�_�{�^��

    '    Private Const BUTTON_INTERVAL As Integer = 50                               ' �R�}���h�{�^���̃{�^���Ԋu
    Private Const BUTTON_INTERVAL As Integer = 45                               ' �R�}���h�{�^���̃{�^���Ԋu

    Private bBlockdataDisp As Boolean = False                                   ' �u���b�N�f�[�^�̕\�����́ATrue
    Private ResistorDisplayNumber As Integer = 1                                ' �\�����̊J�n��R�ԍ�
    Private BlockDisplayNumber As Integer = 0                                   ' �\�����̃u���b�N�ԍ�

    Private Enum StatType
        BLOCK_INITIAL
        BLOCK_FINAL
        PLATE_INITIAL
        PLATE_FINAL
        LOT_INITIAL
        LOT_FINAL
    End Enum

#End Region

#Region "�V���v���g���}��ʏ���������"
    '''=========================================================================
    ''' <summary>
    ''' �V���v���g���}��ʏ���������
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SimpleTrimmerInit()

        Dim setLocation As System.Drawing.Point
        Dim setSize As System.Drawing.Size
        Dim Language As Integer = tkyIni.TMENU.MSGTYP.Get(Of Integer)()
        Dim Location_X As Integer, Location_Y As Integer
        Dim TmpX As Integer, TmpY As Integer, TmpWidth As Integer
        ' ������ V3.1.0.0�B 2014/12/02
        'Dim ObjTkyMsg As DllTkyMsgGet.TkyMsgGet                         ' TKY�p���b�Z�[�W�t�@�C�����[�h�I�u�W�F�N�g(DllTkyMsgGet.dll) 
        Dim strBTN(MAX_FNCNO + 1) As String                             ' �{�^�����̔z��(0 ORG)
        'Dim Count As Integer
        ' ������ V3.1.0.0�B 2014/12/02

        Try

            If gKeiTyp <> KEY_TYPE_RS Then
                Call Form1.SetDataDisplayOff()
                Form1.PanelGraphOnOff(False)                        ' V4.0.0.0�K
                Exit Sub
            End If

            ReDim StatData(83)
            ReDim ResistorData(167)

            ' ������ V3.1.0.0�B 2014/12/02
            'ObjTkyMsg = New DllTkyMsgGet.TkyMsgGet                      ' TKY�p���b�Z�[�W�t�@�C�����[�h�I�u�W�F�N�g���� 
            'Count = ObjTkyMsg.Get_Button_Name(gSysPrm.stTMN.giMsgTyp, strBTN)  ' V4.4.0.0-0
            ' ������ V3.1.0.0�B 2014/12/02

            'V4.0.0.0�K Form1.chkDistributeOnOff.Checked = True     ' ���Y�O���t�@�\��
#If False Then              'V6.0.0.0�C
            ' �N���X���C���T�C�Y�ύX
            ' ��������
            setSize = Form1.VideoLibrary1.Size
            setSize.Height = Form1.Picture1.Height
            Form1.Picture1.Size = setSize
            Form1.Picture1.Location = New Point(Form1.VideoLibrary1.Location.X, Form1.VideoLibrary1.Location.Y + Form1.VideoLibrary1.Size.Width / 2)
            ' ��������
            setSize = Form1.VideoLibrary1.Size
            setSize.Width = Form1.Picture2.Width
            Form1.Picture2.Size = setSize
            Form1.Picture2.Location = New Point(Form1.VideoLibrary1.Location.X + Form1.VideoLibrary1.Size.Width / 2, Form1.VideoLibrary1.Location.Y)
#End If
            ' ���Y�O���t�A�ʒu�A�T�C�Y�ύX
            'V4.0.0.0�K��
            Form1.PanelGraphOnOff(True)                        ' V4.0.0.0�K
            Form1.PanelGraph.Top = Form1.VideoLibrary1.Top + SIMPLE_PICTURE_SIZEY + 5
            Form1.PanelGraph.Left = Form1.VideoLibrary1.Left
            InitializeFormGraphPanel()
            'Form1.cmdGraphSave.Text = PIC_TRIM_10
            'Form1.cmdInitial.Text = PIC_TRIM_01
            'Form1.cmdFinal.Text = PIC_TRIM_02
            frmDistribution.Visible = False
            ' V4.0.0.0�K��

            ' >>> V3.1.0.0�@ 2014/11/28
            'Location_X = setLocation.X + gObjFrmDistribute.Width + 10     ' ���v�f�[�^�\���J�n�w���W
            'Location_Y = setLocation.Y
            Location_X = 0
            Location_Y = 0
            ' <<< V3.1.0.0�@ 2014/11/28
            '            gObjFrmDistribute.Size = setSize

            ' ������ V3.1.0.0�A 2014/12/01
            ' ���O�\���̈ʒu����Ȃ��悤�ɉ��Ɉړ�����
            'Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 10)
            ' ������ V3.1.0.0�C 2014/12/05
            'Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 31)
            '            Form1.txtLog.Location = New Point(setLocation.X, gObjFrmDistribute.Location.Y + gObjFrmDistribute.Height + 11)
            Form1.txtLog.Location = New Point(setLocation.X, Form1.PanelGraph.Location.Y + Form1.PanelGraph.Height + 11)
            ' ������ V3.1.0.0�C 2014/12/05
            ' ������ V3.1.0.0�A 2014/12/01
            setSize = Form1.txtLog.Size
            setSize.Height = Form1.Size.Height - Form1.txtLog.Location.Y
            Form1.txtLog.Size = setSize

            ' ���v�f�[�^��\��
            Form1.frmHistoryData.Visible = False

            ' ���Y�O���t�@�\���A��\���{�^���I�t
            Form1.chkDistributeOnOff.Enabled = False
            Form1.chkDistributeOnOff.Visible = False

            ' �{�^���\���^�u�̕\���ʒu�ύX
            Call GetResDataArea(TmpX, TmpY, TmpWidth)
            Form1.tabCmd.Location = New Point(TmpX, TmpY)
            setSize.Height = Form1.txtLog.Location.Y
            setSize.Width = TmpWidth
            Form1.tabCmd.Size = setSize

            TmpX = (TmpWidth - Form1.CmdLoad.Size.Width) / 2
            TmpY = BUTTON_INTERVAL / 2
            Form1.CmdLoad.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdEdit.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdPattern.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdProbe.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTx.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTy.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdTeach.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            'V4.10.0.0�K                 ��   �����o�^�E��������
            Form1.CmdIntegrated.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            'V4.10.0.0�K                 ��
            Form1.CmdLoging.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdLaser.Location = New Point(TmpX, TmpY)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdSave.Location = New Point(TmpX, TmpY)

            ' ���b�g�ԍ��o�^�{�^���쐬����
            CmdLotNumber = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdLotNumber)
            CmdLotNumber.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdLotNumber.Size = New System.Drawing.Size(145, 30)
            CmdLotNumber.ForeColor = System.Drawing.Color.Black
            CmdLotNumber.Name = "CmdLoad"
            CmdLotNumber.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdLotNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ������ V3.1.0.0 2014/12/02
            'CmdLotNumber.Text = "���b�g�ԍ�"
            'CmdLotNumber.Text = strBTN(F_LOT)
            CmdLotNumber.Text = SimpleTrimmer_036
            ' ������ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdLotNumber.Location = New Point(TmpX, TmpY)

            ' �����^�]
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdAutoOperation.Location = New Point(TmpX, TmpY)

            ' ���[�_������
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdLoaderInit.Location = New Point(TmpX, TmpY)

            ' �����e�i���X�E�{�^���쐬����
            CmdMainte = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdMainte)
            CmdMainte.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdMainte.Size = New System.Drawing.Size(145, 30)
            CmdMainte.ForeColor = System.Drawing.Color.Black
            CmdMainte.Name = "CmdMainte"
            CmdMainte.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdMainte.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ������ V3.1.0.0 2014/12/02
            'CmdMainte.Text = "�����e�i���X"
            'CmdMainte.Text = strBTN(F_MAINTENANCE)
            CmdMainte.Text = SimpleTrimmer_037
            ' ������ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdMainte.Location = New Point(TmpX, TmpY)

            ' �v���[�u�N���[�j���O�E�{�^���쐬����
            CmdProbeClean = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdProbeClean)
            CmdProbeClean.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CmdProbeClean.Size = New System.Drawing.Size(160, 30)
            CmdProbeClean.ForeColor = System.Drawing.Color.Black
            CmdProbeClean.Name = "CmdProbeClean"
            CmdProbeClean.TabIndex = Form1.CmdSave.TabIndex + 1
            CmdProbeClean.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            ' ������ V3.1.0.0 2014/12/02
            'CmdProbeClean.Text = "�v���[�u�N���[�j���O"
            'CmdProbeClean.Text = strBTN(F_PROBE_CLEANING)
            CmdProbeClean.Text = SimpleTrimmer_038
            ' ������ V3.1.0.0 2014/12/02
            TmpY = TmpY + BUTTON_INTERVAL
            CmdProbeClean.Location = New Point(TmpX, TmpY)

            Form1.Controls.Remove(Form1.CmdEnd)
            Form1.tabBaseCmnds.Controls.Add(Form1.CmdEnd)
            TmpY = TmpY + BUTTON_INTERVAL
            Form1.CmdEnd.Location = New Point(TmpX, TmpY)

            ' �u���b�N�f�[�^�\���{�^���mBLOCK DATA�n�쐬����
            CmdBlockData = New System.Windows.Forms.Button
            Form1.tabBaseCmnds.Controls.Add(CmdBlockData)
            CmdBlockData.BackColor = System.Drawing.SystemColors.Control
            CmdBlockData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            CmdBlockData.Cursor = System.Windows.Forms.Cursors.Default
            CmdBlockData.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite
            CmdBlockData.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            CmdBlockData.ForeColor = System.Drawing.SystemColors.ControlText
            CmdBlockData.Location = New System.Drawing.Point(455, 116)
            CmdBlockData.Name = "cmdBlockData"
            CmdBlockData.RightToLeft = System.Windows.Forms.RightToLeft.No
            CmdBlockData.Size = New System.Drawing.Size(145, 30)
            CmdBlockData.TabIndex = 240
            'If Language = 0 Then 'V4.1.0.0�C
            '    CmdBlockData.Text = "�u���b�N�f�[�^"
            'Else
            '    CmdBlockData.Text = "BLOCK DATA"
            'End If
            CmdBlockData.Text = SimpleTrimmer_002
            CmdBlockData.UseVisualStyleBackColor = False
            CmdBlockData.Enabled = True
            CmdBlockData.Visible = False
            TmpY = TmpY + BUTTON_INTERVAL
            CmdBlockData.Location = New Point(TmpX, TmpY)

            'V4.8.0.1�@ ��
            ' �`�F�b�N�{�b�N�X����
            CheckNGRate = New System.Windows.Forms.CheckBox
            Form1.Controls.Add(CheckNGRate)
            CheckNGRate.BackColor = System.Drawing.Color.FromArgb(192, 255, 255)
            CheckNGRate.Size = New System.Drawing.Size(145, 30)
            If Language = 0 Then 'V4.1.0.0�C
                CheckNGRate.Text = "�s�Ǘ�"
            Else
                CheckNGRate.Text = "NG Rate"
            End If
            CheckNGRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            TmpX = 450
            TmpY = 20
            CheckNGRate.Location = New Point(TmpX, TmpY)
            CheckNGRate.ForeColor = System.Drawing.SystemColors.ControlText
            CheckNGRate.BackColor = System.Drawing.SystemColors.Control
            CheckNGRate.Visible = True
            'V4.8.0.1�@ ��

            '----- V6.0.3.0_40 ��-----
            ' �s�Ǘ���\��
            If (giRateDisp = 0) Then
                CheckNGRate.Visible = False
            End If
            '----- V6.0.3.0_40 ��-----

            ' ������ V3.1.0.0 2014/12/02
            ' �㏈��
            'ObjTkyMsg = Nothing                                         ' �I�u�W�F�N�g�J��
            ' ������ V3.1.0.0 2014/12/02

            '-------------------------------------------------------------------------------
            ' ��ʒ����̃f�[�^�\���̈�̕\������
            '-------------------------------------------------------------------------------
            Dim DataWidth1 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE)
            Dim DataWidth2 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE) / 2
            Dim DataWidth4 As Integer = (TKY_DATA_WIDTH - TKY_TITLE_SIZE) / 4

            ' >>> V3.1.0.0�@ 2014/11/28
            ' �p�l���̍����ƕ�
            Form1.pnlDataDisplay.Size = New Size(TKY_DATA_WIDTH, (TKY_DATA_HIGHT * (7 + 4 + 12)) + (TKY_DATA_INTERVAL * (1 + 1)) - TKY_DATA_HIGHT / 2)
            ' <<< V3.1.0.0�@ 2014/11/28

            For Cnt As Integer = 0 To UBound(StatData)
                StatData(Cnt).Label = New TKY_ALL_SL432HW.CustomTkyLabel
                StatData(Cnt).Label.AutoSize = False
                StatData(Cnt).Label.BorderColor = TKY_LINE_COLOR
                StatData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                StatData(Cnt).Label.BorderWidth = TKY_BORDER_WIDTH
                StatData(Cnt).Label.Name = "CustomTkyLabel1"
                StatData(Cnt).Label.Text = ""
                StatData(Cnt).Label.Padding = New System.Windows.Forms.Padding(0)
                StatData(Cnt).Label.TabIndex = 361 + Cnt
                ''V4.0.0.0�L                StatData(Cnt).Label.Font = New System.Drawing.Font("�l�r �S�V�b�N", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                StatData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                StatData(Cnt).Label.ForeColor = System.Drawing.Color.Black
                StatData(Cnt).Label.BackColor = System.Drawing.SystemColors.Window
                StatData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

                Select Case (Cnt)
                    ' �����܂�\��
                    Case 0
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        ''V4.1.0.0�Q                        StatData(Cnt).Label.Size = New System.Drawing.Size(TKY_DATA_WIDTH, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.Size = New System.Drawing.Size((TKY_DATA_WIDTH / 2), TKY_DATA_HIGHT) ''V4.1.0.0�Q
                        StatData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.None
                        StatData(Cnt).Label.BackColor = System.Drawing.SystemColors.Control
                        StatData(Cnt).Label.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
                        StatData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                        'StatData(Cnt).Label.Font = New System.Drawing.Font("�l�r �S�V�b�N", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.Font = New System.Drawing.Font(SimpleTrimmer_003, 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        Location_Y = Location_Y + TKY_DATA_HIGHT
                        StatData(Cnt).Label.Visible = True
                        ' ���̃^�C�g��
                    Case 1, 3, 5, 7, 10, 13, 15, 17, 20, 23, 26, 29, 34, 39, 44, 49, 54, 59, 64, 69, 74, 79
                        If Cnt = 15 Or Cnt = 26 Then
                            Location_Y = Location_Y + TKY_DATA_INTERVAL
                        End If
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(TKY_TITLE_SIZE, TKY_DATA_HIGHT)
                        'StatData(Cnt).Label.Font = New System.Drawing.Font("�l�r �S�V�b�N", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.Font = New System.Drawing.Font(SimpleTrimmer_003, 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        StatData(Cnt).Label.BackColor = Color.LightGreen
                        ' �P��̃f�[�^
                    Case 2, 4, 6, 14, 16
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth1 + TKY_DATA_X_OFF * 3, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.GreenYellow
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �Q��̃f�[�^�̂P���
                    Case 8, 11, 18, 21, 24, 27
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + TKY_DATA_X_OFF, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �Q��̃f�[�^�̂Q���
                    Case 9, 12, 19, 22, 25, 28
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 3 + DataWidth2, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + TKY_DATA_X_OFF, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �S��̃f�[�^�̂P���
                    Case 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �S��̃f�[�^�̂Q���
                    Case 31, 36, 41, 46, 51, 56, 61, 66, 71, 76, 81
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 2 + DataWidth4, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �S��̃f�[�^�̂R���
                    Case 32, 37, 42, 47, 52, 57, 62, 67, 72, 77, 82
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 3 + DataWidth4 * 2, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                        ' �S��̃f�[�^�̂S���
                    Case 33, 38, 43, 48, 53, 58, 63, 68, 73, 78, 83
                        StatData(Cnt).Label.Location = New System.Drawing.Point(Location_X + TKY_TITLE_SIZE + TKY_DATA_X_OFF * 4 + DataWidth4 * 3, Location_Y)
                        StatData(Cnt).Label.Size = New System.Drawing.Size(DataWidth4, TKY_DATA_HIGHT)
                        Location_Y = Location_Y + TKY_DATA_HIGHT + TKY_DATA_Y_OFF
                        StatData(Cnt).Label.BackColor = Color.LemonChiffon
                End Select

                ' >>> V3.1.0.0�@ 2014/11/28
                ' ���C������p�l���֒ǉ�����悤�ɕύX
                'Form1.Controls.Add(StatData(Cnt).Label)
                Form1.pnlDataDisplay.Controls.Add(StatData(Cnt).Label)
                'Form1.pnlDataDisplay.Left = 440    'V4.10.0.0�K
                ' <<< V3.1.0.0�@ 2014/11/28
            Next
            Form1.pnlDataDisplay.Left = 438         'V4.10.0.0�K tabCmd�̍��[���B���̂Œ���

            'V5.0.0.9�O                  ��
            With Form1.GrpStartBlk
                .Location = Point.Subtract(
                    Form1.pnlDataDisplay.PointToScreen(New Point(StatData(2).Label.Right, StatData(2).Label.Top)),
                    .Size)
            End With
            'V5.0.0.9�O                  ��

            ' ���b�g�ԍ��A�J�n���ԁA�o�ߎ��ԁA��
            StatData(0).Label.Text = SimpleTrimmer_004 ' �����܂�
            StatData(1).Label.Text = SimpleTrimmer_005 ' ���b�g�ԍ�
            StatData(3).Label.Text = SimpleTrimmer_006 ' �J�n����
            StatData(5).Label.Text = SimpleTrimmer_007 ' �o�ߎ���
            StatData(7).Label.Text = SimpleTrimmer_008 ' �������
            StatData(10).Label.Text = SimpleTrimmer_009 ' ������R��
            StatData(13).Label.Text = SimpleTrimmer_010 ' ���[�U�p���[

            StatData(15).Label.Text = SimpleTrimmer_011 ' �ڕW�l
            StatData(18).Label.Text = SimpleTrimmer_012 ' ����
            StatData(19).Label.Text = SimpleTrimmer_013 ' ���
            StatData(20).Label.Text = SimpleTrimmer_014 ' ��������
            StatData(23).Label.Text = SimpleTrimmer_015 ' �ŏI����

            StatData(27).Label.Text = SimpleTrimmer_016 ' ���
            StatData(28).Label.Text = SimpleTrimmer_017 ' ���b�g
            StatData(30).Label.Text = SimpleTrimmer_014 ' ��������
            StatData(31).Label.Text = SimpleTrimmer_015 ' �ŏI����
            StatData(32).Label.Text = SimpleTrimmer_014 ' ��������
            StatData(33).Label.Text = SimpleTrimmer_015 ' �ŏI����

            StatData(34).Label.Text = SimpleTrimmer_018 ' ������
            StatData(39).Label.Text = SimpleTrimmer_019 ' �n�j(%)
            StatData(44).Label.Text = SimpleTrimmer_020 ' Low NG(%)
            StatData(49).Label.Text = SimpleTrimmer_021 ' High NG(%)
            StatData(54).Label.Text = SimpleTrimmer_022 ' Range Over(%)
            StatData(59).Label.Text = SimpleTrimmer_023 ' �ŏ��l
            StatData(64).Label.Text = SimpleTrimmer_024 ' �ő�l
            StatData(69).Label.Text = SimpleTrimmer_025 ' ���ϒl
            StatData(74).Label.Text = SimpleTrimmer_026 ' �R��
            StatData(79).Label.Text = SimpleTrimmer_027 ' �b���j
            If giCpk_Disp_Off Then                      'V5.0.0.4�C
                StatData(79).Label.Text = "" ' �b���j    V5.0.0.4�C
            End If                                      'V5.0.0.4�C
            'If Language = 0 Then
            '    StatData(0).Label.Text = "�����܂�"
            '    StatData(1).Label.Text = "���b�g�ԍ�"
            '    StatData(3).Label.Text = "�J�n����"
            '    StatData(5).Label.Text = "�o�ߎ���"
            '    StatData(7).Label.Text = "�������"
            '    StatData(10).Label.Text = "������R��"
            '    StatData(13).Label.Text = "���[�U�p���["

            '    StatData(15).Label.Text = "�ڕW�l"
            '    StatData(18).Label.Text = "����"
            '    StatData(19).Label.Text = "���"
            '    StatData(20).Label.Text = "��������"
            '    StatData(23).Label.Text = "�ŏI����"

            '    StatData(27).Label.Text = "���"
            '    StatData(28).Label.Text = "���b�g"
            '    StatData(30).Label.Text = "��������"
            '    StatData(31).Label.Text = "�ŏI����"
            '    StatData(32).Label.Text = "��������"
            '    StatData(33).Label.Text = "�ŏI����"

            '    StatData(34).Label.Text = "������"
            '    StatData(39).Label.Text = "�n�j(%)"
            '    StatData(44).Label.Text = "Low NG(%)"
            '    StatData(49).Label.Text = "High NG(%)"
            '    StatData(54).Label.Text = "Range Over(%)"
            '    StatData(59).Label.Text = "�ŏ��l"
            '    StatData(64).Label.Text = "�ő�l"
            '    StatData(69).Label.Text = "���ϒl"
            '    StatData(74).Label.Text = "�R��"
            '    StatData(79).Label.Text = "�b���j"
            'Else
            '    StatData(0).Label.Text = "YIELD"
            '    StatData(1).Label.Text = "LOT NO"
            '    StatData(3).Label.Text = "START TIME"
            '    StatData(5).Label.Text = "ELAPSED TIME"
            '    StatData(7).Label.Text = "Substrates"
            '    StatData(10).Label.Text = "Resistors"
            '    StatData(13).Label.Text = "Laser Power"

            '    StatData(15).Label.Text = "Nominal"
            '    StatData(18).Label.Text = "Min"
            '    StatData(19).Label.Text = "Max"
            '    StatData(20).Label.Text = "Initial Test"
            '    StatData(23).Label.Text = "Final Test"

            '    StatData(27).Label.Text = "Substrate"
            '    StatData(28).Label.Text = "Lot"
            '    StatData(30).Label.Text = "Initial Test"
            '    StatData(31).Label.Text = "Final Test"
            '    StatData(32).Label.Text = "Initial Test"
            '    StatData(33).Label.Text = "Final Test"

            '    StatData(34).Label.Text = "Resistors"
            '    StatData(39).Label.Text = "OK(%)"
            '    StatData(44).Label.Text = "Low NG(%)"
            '    StatData(49).Label.Text = "High NG(%)"
            '    StatData(54).Label.Text = "Range Over(%)"
            '    StatData(59).Label.Text = "Min"
            '    StatData(64).Label.Text = "Max"
            '    StatData(69).Label.Text = "Average"
            '    StatData(74).Label.Text = "3��"
            '    StatData(79).Label.Text = "CpK"
            'End If
            '-------------------------------------------------------------------------------
            ' ��ʒ����̃f�[�^�\���̈�̕\�������̏I��
            '-------------------------------------------------------------------------------

            ' >>> V3.1.0.0�@ 2014/11/28
            Location_X = setLocation.X + Form1.PanelGraph.Width + 10     ' ���v�f�[�^�\���J�n�w���W
            Location_Y = 788
            ' <<< V3.1.0.0�@ 2014/11/28

            ' �R�}���h�{�^���\���T�C�Y�ƈʒu�̕ύX
            Form1.GrpMode.Location = New Point(Location_X, Location_Y)
            '            setSize.Height = Form1.txtLog.Location.Y - Location_Y - 6
            ' ������ V3.1.0.0�A 2014/12/01
            'setSize.Height = Form1.txtLog.Location.Y - Location_Y + 20
            setSize.Height = Form1.txtLog.Location.Y - Location_Y - 1
            ' ������ V3.1.0.0�A 2014/12/01
            setSize.Width = TKY_DATA_WIDTH
            Form1.GrpMode.Size = setSize

            ' [DATA CLR]��[DATA SAVE]�{�^���̕\�� 
            DataClrButton = New System.Windows.Forms.Button
            DataSaveButton = New System.Windows.Forms.Button
            BPAdjustButton = New System.Windows.Forms.Button
            DataEditButton = New System.Windows.Forms.Button            'V4.0.0.0-84
            Form1.GrpMode.Controls.Add(DataClrButton)
            Form1.GrpMode.Controls.Add(DataSaveButton)
            Form1.GrpMode.Controls.Add(BPAdjustButton)
            Form1.GrpMode.Controls.Add(DataEditButton)
            ' SimpleTrimmer_003 = �l�r �S�V�b�N
            DataClrButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            DataSaveButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BPAdjustButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            DataEditButton.Font = New System.Drawing.Font(SimpleTrimmer_003, 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte)) 'V4.0.0.0-84
            DataClrButton.TextAlign = ContentAlignment.MiddleCenter
            DataSaveButton.TextAlign = ContentAlignment.MiddleCenter
            BPAdjustButton.TextAlign = ContentAlignment.MiddleCenter
            DataEditButton.TextAlign = ContentAlignment.MiddleCenter 'V4.0.0.0-84
            'If Language = 0 Then 'V4.1.0.0�C
            '    DataClrButton.Text = "�f�[�^�N���A"
            '    DataSaveButton.Text = "�f�[�^�Z�[�u"
            '    BPAdjustButton.Text = "BP ����"
            '    DataEditButton.Text = "�f�[�^�ҏW" 'V4.0.0.0-84
            'Else
            '    DataClrButton.Text = "DATA CLR"
            '    DataSaveButton.Text = "DATA SAVE"
            '    BPAdjustButton.Text = "BP ADJUST"
            '    DataEditButton.Text = "DATA EDIT" 'V4.0.0.0-84
            'End If
            DataClrButton.Text = SimpleTrimmer_028 ' �f�[�^�N���A
            DataSaveButton.Text = SimpleTrimmer_029 ' �f�[�^�Z�[�u
            BPAdjustButton.Text = SimpleTrimmer_030 ' BP ����
            DataEditButton.Text = SimpleTrimmer_031 ' �f�[�^�ҏW 'V4.0.0.0-84

            DataClrButton.Size = New System.Drawing.Size(143, 29)
            DataSaveButton.Size = New System.Drawing.Size(143, 29)
            BPAdjustButton.Size = New System.Drawing.Size(143, 29)
            DataEditButton.Size = New System.Drawing.Size(143, 29) 'V4.0.0.0-84
            DataClrButton.Location = New Point(Form1.LblDIGSW_HI.Location.X, Form1.LblDIGSW_HI.Location.Y + (Form1.LblDIGSW.Location.Y - Form1.LblDIGSW_HI.Location.Y) * 2)
            DataSaveButton.Location = New Point(DataClrButton.Location.X + DataClrButton.Size.Width + 12, DataClrButton.Location.Y)
            BPAdjustButton.Location = New Point(DataClrButton.Location.X + DataClrButton.Size.Width + 12, DataClrButton.Location.Y + 30)
            DataEditButton.Location = New Point(DataClrButton.Location.X, DataClrButton.Location.Y + 30) 'V4.0.0.0-84

            Form1.BtnADJ.Location = New Point(DataSaveButton.Location.X + DataSaveButton.Size.Width + 12, DataClrButton.Location.Y)
            BPAdjustButton.Visible = False
            DataEditButton.Visible = False 'V4.0.0.0-84
            Form1.btnCycleStop.Location = New Point(Form1.BtnADJ.Location.X, Form1.BtnADJ.Location.Y + Form1.BtnADJ.Size.Height)   'V5.0.0.4�@

            '----- V4.11.0.0�E�� (WALSIN�aSL436S�Ή�) -----
            ' �u������{�^���v�\���ʒu�ݒ�
            TmpX = Form1.BtnADJ.Location.X
            TmpY = Form1.BtnADJ.Location.Y + Form1.BtnADJ.Size.Height + 2
            Form1.BtnSubstrateSet.Location = New Point(TmpX, TmpY)
            '----- V4.11.0.0�E�� -----

            ' �}�K�W��UP/Down�@�\�̈ʒu�C��
            Form1.GroupBox1.Left = Form1.tabCmd.Left
            Form1.GroupBox1.Top = Form1.tabCmd.Top + Form1.tabCmd.Height + 5
            Form1.txtLog.Width = Form1.txtLog.Width - 200

            ' QR�R�[�h/�o�[�R�[�h���\���ʒu�ݒ�
            'Form1.GrpQrCode.Top = Form1.GrpMode.Top + Form1.GrpMode.Height + 10
            'Form1.GrpQrCode.Left = StatData(0).Label.Left
            Form1.GrpQrCode.Top = Form1.GrpMode.Top + Form1.GrpMode.Height
            Form1.GrpQrCode.Left = Form1.GrpMode.Left

            '----- V4.11.0.0�A�� (WALSIN�aSL436S�Ή�) -----
            ' �o�[�R�[�h���T�C�Y�ݒ�
            'V5.0.0.9�N            If (gSysPrm.stCTM.giSPECIAL = customWALSIN) Then
            If (BarcodeType.Walsin = BarCode_Data.Type) Then            'V5.0.0.9�N
                Form1.GrpQrCode.Size = New Size(220, 50)
                Form1.lblQRData.Size = New Size(210, 32)
            End If
            '----- V4.11.0.0�A�� -----

            'TrimData = New TrimClassLibrary.TrimData()  'V4.3.0.0�A

            'V4.0.0.0�L
            Dim dspOfs As Integer = -2
            ' ����f�[�^�\���p�u���b�N�ԍ��̑O��\���ؑփ{�^���̕\�� 
            ' ���u���b�N�ړ��{�^���̒ǉ�
            BlockNextButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockNextButton)
            BlockNextButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockNextButton.TextAlign = ContentAlignment.MiddleCenter
            BlockNextButton.Text = "Next"

            BlockNextButton.Size = New System.Drawing.Size(70, 25)
            BlockNextButton.Location = New Point(Form1.LblRotAtt.Location.X + Form1.LblRotAtt.Width + 10, Form1.LblRotAtt.Location.Y + dspOfs)
            BlockNextButton.Visible = False

            ' ����f�[�^�\���p�u���b�N�ԍ����x���̒ǉ�
            BlockNoLabel = New System.Windows.Forms.Label
            Form1.Controls.Add(BlockNoLabel)
            BlockNoLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockNoLabel.TextAlign = ContentAlignment.MiddleCenter
            BlockNoLabel.Text = ""
            BlockNoLabel.Size = New System.Drawing.Size(40, 25)
            'V5.0.0.9�O            BlockNoLabel.Location = New Point(BlockNextButton.Left + BlockNextButton.Width, Form1.LblRotAtt.Location.Y)
            BlockNoLabel.Location = New Point(BlockNextButton.Left + BlockNextButton.Width, BlockNextButton.Top) 'V5.0.0.9�O
            BlockNoLabel.Visible = False

            ' �O�u���b�N�ړ��{�^���̒ǉ�
            BlockRvsButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockRvsButton)
            BlockRvsButton.TextAlign = ContentAlignment.MiddleCenter
            BlockRvsButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            BlockRvsButton.Text = "Prev"
            BlockRvsButton.Size = New System.Drawing.Size(70, 25)
            'V5.0.0.9�O            BlockRvsButton.Location = New Point(BlockNoLabel.Location.X + BlockNoLabel.Size.Width + 5, BlockNoLabel.Location.Y + dspOfs)
            BlockRvsButton.Location = New Point(BlockNoLabel.Location.X + BlockNoLabel.Size.Width + 5, BlockNextButton.Top) 'V5.0.0.9�O
            BlockRvsButton.Visible = False

            'V4.0.0.0-76
            ' �O�u���b�N�ړ��{�^���̒ǉ�
            BlockMainButton = New System.Windows.Forms.Button
            Form1.Controls.Add(BlockMainButton)
            BlockMainButton.TextAlign = ContentAlignment.MiddleCenter
            BlockMainButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            'If Language = 0 Then 'V4.1.0.0�C
            '    BlockMainButton.Text = "���C��"
            'Else
            '    BlockMainButton.Text = "Main"
            'End If
            BlockMainButton.Text = SimpleTrimmer_032 ' ���C��
            BlockMainButton.Size = New System.Drawing.Size(70, 25)
            'V5.0.0.9�O            BlockMainButton.Location = New Point(BlockNextButton.Left - 90, Form1.LblRotAtt.Location.Y + dspOfs)
            BlockMainButton.Location = New Point(Form1.tabCmd.Left, Form1.LblRotAtt.Location.Y + dspOfs) 'V5.0.0.9�O
            BlockMainButton.Visible = False

            'V4.0.0.0-76

            ' ���肵���Ƃ��̃g���~���O���[�h�\���f�o�b�O�p
            'BlockTrimModeLabel = New System.Windows.Forms.Label
            'Form1.Controls.Add(BlockTrimModeLabel)
            'BlockTrimModeLabel.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            'BlockTrimModeLabel.TextAlign = ContentAlignment.MiddleCenter
            'BlockTrimModeLabel.Text = "A"
            'BlockTrimModeLabel.Size = New System.Drawing.Size(40, 25)
            'BlockTrimModeLabel.Location = New Point(BlockNextButton.Left - 50, Form1.LblRotAtt.Location.Y)
            'BlockTrimModeLabel.Visible = False
            'BlockTrimModeLabel.Visible = True

            'V4.0.0.0�L

            ' ���݂̃u���b�N�ԍ��\���p���x���̒ǉ�
            NowBlockNoLabel = New System.Windows.Forms.Label
            Form1.pnlDataDisplay.Controls.Add(NowBlockNoLabel)
            NowBlockNoLabel.Font = New System.Drawing.Font("MS UI Gothic", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            NowBlockNoLabel.TextAlign = ContentAlignment.MiddleCenter
            NowBlockNoLabel.Text = ""
            NowBlockNoLabel.Size = New System.Drawing.Size(140, 25)
            NowBlockNoLabel.Location = New Point((TKY_DATA_WIDTH - 150), StatData(0).Label.Location.Y)
            'NowBlockNoLabel.Location = New Point((TKY_DATA_WIDTH - 150), BlockMainButton.Location.Y + dspOfs) 
            NowBlockNoLabel.Visible = False

#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                SoftStartButton = New System.Windows.Forms.Button
                Form1.Controls.Add(SoftStartButton)
                SoftStartButton.TextAlign = ContentAlignment.MiddleCenter
                SoftStartButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                SoftStartButton.Text = "START"
                SoftStartButton.Size = New System.Drawing.Size(100, 50)
                SoftStartButton.Location = New Point(Form1.GrpQrCode.Left + Form1.GrpQrCode.Width + 12, Form1.GrpQrCode.Top + 12)
                SoftStartButton.BackColor = System.Drawing.Color.LimeGreen
                'SoftStartButton.Location = New Point(0, 0)
                SoftStartButton.Visible = True

                SoftResetButton = New System.Windows.Forms.Button
                Form1.Controls.Add(SoftResetButton)
                SoftResetButton.TextAlign = ContentAlignment.MiddleCenter
                SoftResetButton.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                SoftResetButton.Text = "RESET"
                SoftResetButton.Size = New System.Drawing.Size(100, 50)
                SoftResetButton.Location = New Point(SoftStartButton.Location.X + SoftStartButton.Size.Width + 12, SoftStartButton.Location.Y)
                SoftResetButton.BackColor = System.Drawing.Color.Peru
                SoftResetButton.Visible = True
            End If
#End If

            Call ResistorDataInitialDisp()

            'V4.0.0.0�K��
            gObjFrmDistribute.RedrawGraph()
            'V4.0.0.0�K��

            SetSimpleVideoSize()

            'V4.0.0.0�K��
            ' �Ƽ��ý�/̧���ýĕ��z�}���x����ݒ肷��
            'Form1.Label18.Text = PIC_TRIM_09                   ' "��R��"
            'Form1.Label2.Text = PIC_TRIM_03                     ' "�Ǖi"
            'Form1.Label3.Text = PIC_TRIM_04                       ' "�s�Ǖi"
            'Form1.Label4.Text = PIC_TRIM_05                      ' "�ŏ�%"
            'Form1.Label5.Text = PIC_TRIM_06                      ' "�ő�%"
            'Form1.Label6.Text = PIC_TRIM_07                       ' "����%"
            'Form1.Label7.Text = PIC_TRIM_08                     ' "�W���΍�"
            'V4.0.0.0�K��


            'V4.1.0.0�D
            Form1.ChkButtonDisp()
            'V4.1.0.0�D

            'V4.10.0.0�@              ��
            With Form1.tabCmd
                Form1.btnUserLogon.Location = New Point(.Left, .Top + .Height - Form1.btnUserLogon.Height - 10)
                Form1.btnUserLogon.Size = New Size(.Width - 2, Form1.btnUserLogon.Height)
            End With
            'V4.10.0.0�@              ��

            Form1.Refresh()
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrimmerInit() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "��R�f�[�^�\���J�n�ʒu���W�ƕ��̎擾"
    Private Sub GetResDataArea(ByRef X As Integer, ByRef Y As Integer, ByRef Width As Integer)
        Dim setLocation As System.Drawing.Point
        Dim setSize As System.Drawing.Size

        Try
            setLocation = Form1.VideoLibrary1.Location
            setSize = Form1.VideoLibrary1.Size

            X = setLocation.X + Form1.PanelGraph.Width + TKY_DATA_WIDTH + 6     ' �f�[�^�\���J�n�w���W
            Y = setLocation.Y
            setSize = Form1.Size
            Width = setSize.Width - X - 2   ' 8�́A��ʉE���̗]��
        Catch ex As Exception
            MsgBox("SimpleTrimmer.GetResDataArea() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "��R�f�[�^�����\������"
    ''' <summary>
    ''' ��R�f�[�^�����\������
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResistorDataInitialDisp()
        Dim Location_X As Integer, Location_Y As Integer
        Dim CntB As Integer
        Dim DataWidth2 As Integer
        Dim CntMax As Integer = UBound(ResistorData)
        Dim Language As Integer = tkyIni.TMENU.MSGTYP.Get(Of Integer)()
        Dim ResDataWidth As Integer         ' �f�[�^�S�̕�
        Dim bFirst As Boolean = False

        Try
            If HeaderButton(0) Is Nothing Then
                For Cnt As Integer = 0 To 2
                    HeaderButton(Cnt) = New System.Windows.Forms.Label
                    Form1.Controls.Add(HeaderButton(Cnt))
                Next
            End If

            Call GetResDataArea(Location_X, Location_Y, ResDataWidth)
            DataWidth2 = (ResDataWidth - RES_TITLE_SIZE) / 2

            For Cnt As Integer = 0 To CntMax
                If ResistorData(Cnt).Label Is Nothing Then
                    bFirst = True
                Else
                    Continue For
                End If
                ResistorData(Cnt).Label = New TKY_ALL_SL432HW.CustomTkyLabel
                ResistorData(Cnt).Label.AutoSize = False
                ResistorData(Cnt).Label.BorderColor = RES_LINE_COLOR
                ResistorData(Cnt).Label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                ResistorData(Cnt).Label.BorderWidth = RES_BORDER_WIDTH
                ResistorData(Cnt).Label.Name = "Resistor"
                ResistorData(Cnt).Label.Text = ""
                ResistorData(Cnt).Label.Padding = New System.Windows.Forms.Padding(0)
                ResistorData(Cnt).Label.TabIndex = 361 + Cnt
                ResistorData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                ResistorData(Cnt).Label.ForeColor = System.Drawing.Color.Black
                ResistorData(Cnt).Label.BackColor = System.Drawing.SystemColors.Window
                ResistorData(Cnt).Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                ResistorData(Cnt).Label.Visible = False

                If Cnt >= RES_DATA_START Then
                    AddHandler ResistorData(Cnt).Label.Click, AddressOf ResistorData_Click_NextResistor
                Else
                    AddHandler HeaderButton(Cnt).Click, AddressOf ResistorData_Click_BlockChange
                End If

                CntB = Cnt Mod 3

                Select Case (CntB)
                    ' �����܂�\��
                    Case 0

                        ResistorData(Cnt).Label.Text = (Cnt / 3).ToString()
                        If Cnt = 153 Then
                            Location_Y = Location_Y + RES_DATA_INTERVAL
                        End If
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(RES_TITLE_SIZE, RES_DATA_HIGHT)
                        ResistorData(Cnt).Label.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                        ' �Q��̃f�[�^�̂P���
                    Case 1
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X + RES_TITLE_SIZE + RES_DATA_X_OFF, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + RES_DATA_X_OFF, RES_DATA_HIGHT)
                        ' �Q��̃f�[�^�̂Q���
                    Case 2
                        ResistorData(Cnt).Label.Location = New System.Drawing.Point(Location_X + RES_TITLE_SIZE + RES_DATA_X_OFF * 3 + DataWidth2, Location_Y)
                        ResistorData(Cnt).Label.Size = New System.Drawing.Size(DataWidth2 + RES_DATA_X_OFF, RES_DATA_HIGHT)
                        Location_Y = Location_Y + RES_DATA_HIGHT + RES_DATA_Y_OFF
                End Select
                Form1.Controls.Add(ResistorData(Cnt).Label)
            Next
            If bFirst Then
                'If Language = 0 Then
                '    ResistorData(0).Label.Text = "��R"
                '    ResistorData(1).Label.Text = "��������i�덷%�j"
                '    ResistorData(2).Label.Text = "�ŏI����i�덷%�j"
                '    ResistorData(RES_DATA_STAT).Label.Text = "�ŏ��l"
                '    ResistorData(RES_DATA_STAT + 3).Label.Text = "�ő�l"
                '    ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = "���ϒl"
                '    ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = "�R��"
                '    ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = "�b���j"
                'Else
                '    ResistorData(0).Label.Text = "Res No"
                '    ResistorData(1).Label.Text = "Initial Test�iDEV%�j"
                '    ResistorData(2).Label.Text = "Final Test�iDEV%�j"
                '    ResistorData(RES_DATA_STAT).Label.Text = "Min"
                '    ResistorData(RES_DATA_STAT + 3).Label.Text = "Max"
                '    ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = "Average"
                '    ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = "3sigma"
                '    ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = "CpK"
                'End If
                ResistorData(0).Label.Text = SimpleTrimmer_033 ' ��R
                ResistorData(1).Label.Text = SimpleTrimmer_034 ' ��������i�덷%�j
                ResistorData(2).Label.Text = SimpleTrimmer_035 ' �ŏI����i�덷%�j
                ResistorData(RES_DATA_STAT).Label.Text = SimpleTrimmer_023 ' �ŏ��l
                ResistorData(RES_DATA_STAT + 3).Label.Text = SimpleTrimmer_024 ' �ő�l
                ResistorData(RES_DATA_STAT + 3 * 2).Label.Text = SimpleTrimmer_025 ' ���ϒl
                ResistorData(RES_DATA_STAT + 3 * 3).Label.Text = SimpleTrimmer_026 ' �R��
                ResistorData(RES_DATA_STAT + 3 * 4).Label.Text = SimpleTrimmer_027 ' �b���j
                For Cnt As Integer = 0 To 2

                    HeaderButton(Cnt).BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                    HeaderButton(Cnt).Location = New System.Drawing.Point(ResistorData(Cnt).Label.Location.X, ResistorData(Cnt).Label.Location.Y)
                    HeaderButton(Cnt).Size = New System.Drawing.Size(ResistorData(Cnt).Label.Size.Width, ResistorData(Cnt).Label.Size.Height)
                    HeaderButton(Cnt).Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
                    HeaderButton(Cnt).Text = ResistorData(Cnt).Label.Text
                    HeaderButton(Cnt).FlatStyle = FlatStyle.Standard
                    HeaderButton(Cnt).TabIndex = 362 + CntMax + Cnt
                    HeaderButton(Cnt).ForeColor = System.Drawing.Color.Black
                    HeaderButton(Cnt).BackColor = System.Drawing.SystemColors.ButtonFace
                    HeaderButton(Cnt).TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                    HeaderButton(Cnt).Enabled = False
                    HeaderButton(Cnt).Visible = False

                    'HeaderButton(Cnt).ForeColor = System.Drawing.Color.Black
                    'HeaderButton(Cnt).BackColor = System.Drawing.SystemColors.Window
                Next
            End If

            'If TrimData.IsTrimmingStopStatus Then
            '    For Cnt As Integer = 0 To 2
            '        ResistorData(Cnt).Label.Enabled = False
            '        ResistorData(Cnt).Label.Visible = False
            '        HeaderButton(Cnt).Enabled = True
            '        HeaderButton(Cnt).Visible = True
            '    Next
            'Else
            '    For Cnt As Integer = 0 To 2
            '        HeaderButton(Cnt).Enabled = False
            '        HeaderButton(Cnt).Visible = False
            '        ResistorData(Cnt).Label.Enabled = True
            '        ResistorData(Cnt).Label.Visible = True
            '    Next
            'End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistoreDataDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Private Sub ResistorData_Click_BlockChange(ByVal sender As Object, ByVal e As EventArgs)
        Debug.WriteLine("��R�f�[�^�N���b�N�C�x���g����")
        Dim frmBlockDsp As FrmBlockDisp = New FrmBlockDisp()

        frmBlockDsp.Show()

    End Sub

    Private Sub ResistorData_Click_NextResistor(ByVal sender As Object, ByVal e As EventArgs)
        Debug.WriteLine("��R�f�[�^�N���b�N2�C�x���g����")
        ResistorDisplayNumber = ResistorDisplayNumber + RES_TOTAL_COUNTER
        If Not SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber) Then
            ResistorDisplayNumber = 1
            Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
        End If

    End Sub
#End Region

#Region "��R�f�[�^�\������"
    ''' <summary>
    ''' ��R�f�[�^�\������
    ''' </summary>
    ''' <param name="bDisp">True�F�\������ False:��\������</param>
    ''' <param name="BlockNo">�u���b�N�ԍ� 0:���ݏ������̃u���b�N��\��</param>
    ''' <param name="ResNo">��R�ԍ�</param>
    ''' <returns>True;�\���f�[�^�L�� False:�\���f�[�^����</returns>
    ''' <remarks></remarks>
    Public Function ResistorDataDisp(ByVal bDisp As Boolean, ByVal BlockNo As Integer, ByVal ResNo As Integer) As Boolean
        Dim CntMax As Integer = UBound(ResistorData)

        ResistorDataDisp = False
        Try

            ' �f�W�X�C�b�`�̓ǎ��
            Dim digL As Integer, digH As Integer, digSW As Integer
            Call Form1.GetMoveMode(digL, digH, digSW)
            Dim cDummy As String = ""
            Dim FinalJudge As Integer = TrimClassLibrary.TrimDef.TRIM_RESULT_OK
            Dim bRtn As Boolean
            Dim bNgOnly As Boolean
            Dim NgResNo As Integer

            If digH = 0 Then        ' �\������
                bDisp = False  'V4.0.0.0-46
                'V4.0.0.0-46                Exit Function
            ElseIf digH = 1 Then    ' �m�f�̂ݕ\��
                bNgOnly = True
            Else                    ' �S�ĕ\��
                bNgOnly = False
            End If

            If CntMax <> 0 Then
                If bDisp = False Then
                    'V4.0.0.0�L
                    BlockNoLabel.Visible = False
                    BlockNextButton.Visible = False
                    BlockRvsButton.Visible = False
                    Form1.GroupBox1.Visible = True
                    BlockMainButton.Visible = False 'V4.0.0.0-76
                    'V4.0.0.0�L
                Else
                    'V4.0.0.0�L
                    BlockNoLabel.Visible = True
                    BlockNextButton.Visible = True
                    BlockRvsButton.Visible = True
                    Form1.GroupBox1.Visible = False
                    BlockMainButton.Visible = True 'V4.0.0.0-76
                    'V4.0.0.0�L
                End If
            End If

            For Cnt As Integer = 0 To CntMax
                If bDisp = False Then
                    'If Not bBlockdataDisp Then
                    '    Exit Function
                    'End If
                    If ResistorData(Cnt).Label Is Nothing Then
                        Exit Function
                    Else
                        ResistorData(Cnt).Label.Visible = False
                        ''V4.0.0.0�L
                        'BlockNoLabel.Visible = False
                        'BlockNextButton.Visible = False
                        'BlockRvsButton.Visible = False
                        'Form1.GroupBox1.Visible = True
                        'BlockMainButton.Visible = False 'V4.0.0.0-76
                        ''V4.0.0.0�L
                    End If
                    Continue For
                Else
                    ResistorData(Cnt).Label.Visible = True
                    ''V4.0.0.0�L
                    'BlockNoLabel.Visible = True
                    'BlockNextButton.Visible = True
                    'BlockRvsButton.Visible = True
                    'Form1.GroupBox1.Visible = False
                    'BlockMainButton.Visible = True 'V4.0.0.0-76
                    ''V4.0.0.0�L
                End If

                If (0 < Cnt And Cnt < RES_DATA_STAT) And (Cnt Mod 3) = 0 Then
                    ResistorData(Cnt + 1).Label.Text = ""
                    ResistorData(Cnt + 2).Label.Text = ""
                    bRtn = False

                    'V4.0.0.0-23�@��
                    bRtn = TrimData.GetResTrimModeData(BlockNo, ResNo, digL)
                    If (bRtn) Then
                        'V4.0.0.0-23 ��
                        If digL = TRIM_MODE_ITTRFT Then
                            bRtn = TrimData.GetResDispData(BlockNo, ResNo, bNgOnly, NgResNo, gsEDIT_DIGITNUM, "0.00", ResistorData(Cnt + 1).Label.Text, ResistorData(Cnt + 2).Label.Text, FinalJudge)
                            If bRtn Then
                                ResistorDataDisp = True
                            End If
                            If bRtn Then
                                ' ������ V3.1.0.0�C 2014/12/05
                                'Select Case (FinalJudge)
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_OK
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_IT_NG, TrimClassLibrary.TrimDef.TRIM_RESULT_IT_HING, TrimClassLibrary.TrimDef.TRIM_RESULT_IT_LONG
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '    Case Else
                                '        ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End Select
                                ' �C�j�V�����e�X�g�͈̓`�F�b�N
                                ''V5.0.0.4�D��
                                If IsResistorRange(0, ResistorData(Cnt + 1).Label.Text) = True Then
                                    ''V5.0.0.4�D    ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                Else
                                    ''V5.0.0.4�D    ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                End If
                                If (IsTrimJudge(0, ResNo) = True) Then
                                    ResistorData(Cnt + 1).Label.BackColor = Color.GreenYellow
                                Else
                                    ResistorData(Cnt + 1).Label.BackColor = Color.Red
                                End If
                                ''V5.0.0.4�D��

                                ' �t�@�C�i���e�X�g�͈̓`�F�b�N
                                ''V5.0.0.4�D��
                                If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                    ''V5.0.0.4�D                                    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                Else
                                    ''V5.0.0.4�D                                    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                End If
                                If (IsTrimJudge(1, ResNo) = True) Then
                                    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                Else
                                    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                End If
                                ''V5.0.0.4�D��

                                ' ������ V3.1.0.0�C 2014/12/05
                            Else
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                            End If
                        ElseIf digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS Then
                            bRtn = TrimData.GetResDispData(BlockNo, ResNo, bNgOnly, NgResNo, gsEDIT_DIGITNUM, "0.00", cDummy, ResistorData(Cnt + 2).Label.Text, FinalJudge)
                            If bRtn Then
                                ResistorDataDisp = True
                            End If
                            If bRtn Then
                                'V4.1.0.0�D
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                '' '' ''V4.0.0.0-22�@��
                                '' ''ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '' '' ''V4.0.0.0-22�@��
                                'V4.1.0.0�D

                                ' ������ V3.1.0.0�C 2014/12/05
                                'Select Case (FinalJudge)
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_NOTDO
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.White
                                '    Case TrimClassLibrary.TrimDef.TRIM_RESULT_OK
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                '    Case Else
                                '        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End Select

                                ' �t�@�C�i���e�X�g�͈̓`�F�b�N
                                '----- V4.0.0.0�S�� -----
                                'If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                '    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                'Else
                                '    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                'End If
                                ' ������ V3.1.0.0�C 2014/12/05
                                ' x3���[�h�ȊO�͔͈̓`�F�b�N���s��
                                If digL <> TRIM_MODE_MEAS Then
                                    ''V5.0.0.4�D��
                                    If IsResistorRange(1, ResistorData(Cnt + 2).Label.Text) = True Then
                                        'V5.0.0.4�D    ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                    Else
                                        'V5.0.0.4�D    ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                    End If
                                    If (IsTrimJudge(1, ResNo) = True) Then
                                        ResistorData(Cnt + 2).Label.BackColor = Color.GreenYellow
                                    Else
                                        ResistorData(Cnt + 2).Label.BackColor = Color.Red
                                    End If
                                    ''V5.0.0.4�D��

                                Else
                                    'V4.1.0.0�D �ꏊ�ύX 
                                    ''V4.0.0.0-22�@��
                                    ResistorData(Cnt + 2).Label.BackColor = Color.White
                                    ''V4.0.0.0-22�@��
                                    'V4.1.0.0�D
                                End If
                                '----- V4.0.0.0�S�� -----
                            Else
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                            End If
                        End If
                        If bNgOnly Then
                            'V4.0.0.0-47
                            If NgResNo <> 0 Then
                                ResistorData(Cnt).Label.Text = NgResNo.ToString()
                            Else
                                ResistorData(Cnt).Label.Text = ""
                                ResistorData(Cnt + 1).Label.BackColor = Color.White
                                ResistorData(Cnt + 1).Label.Text = ""
                                ResistorData(Cnt + 2).Label.BackColor = Color.White
                                ResistorData(Cnt + 2).Label.Text = ""
                            End If
                            'V4.0.0.0-47

                        Else
                            ResistorData(Cnt).Label.Text = ResNo.ToString()
                        End If
                        ResNo = ResNo + 1
                    Else 'V4.0.0.0-23
                        ResistorData(Cnt).Label.Text = ""
                        ResistorData(Cnt + 1).Label.BackColor = Color.White
                        ResistorData(Cnt + 2).Label.BackColor = Color.White
                        'V4.0.0.0-23
                    End If
                    'BlockTrimModeLabel.Text = digL.ToString()
                End If
            Next

            If bDisp = False Then
                For Cnt As Integer = 0 To 2
                    HeaderButton(Cnt).Enabled = False
                    HeaderButton(Cnt).Visible = False
                    ResistorData(Cnt).Label.Enabled = False
                    ResistorData(Cnt).Label.Visible = False
                Next
            Else
                If TrimData.IsTrimmingStopStatus And TrimData.GetPlateCounter() > 0 Then    ' ����P���ł��������Ă����~��
                    For Cnt As Integer = 0 To 2
                        ResistorData(Cnt).Label.Enabled = False
                        ResistorData(Cnt).Label.Visible = False
                        HeaderButton(Cnt).Enabled = True
                        HeaderButton(Cnt).Visible = True
                    Next
                Else
                    For Cnt As Integer = 0 To 2
                        HeaderButton(Cnt).Enabled = False
                        HeaderButton(Cnt).Visible = False
                        ResistorData(Cnt).Label.Enabled = True
                        ResistorData(Cnt).Label.Visible = True
                    Next
                End If
                'V4.0.0.0�L��
                If BlockNo = 0 Then
                    BlockDisplayNumber = TrimData.GetBlockNumber()
                    BlockNoLabel.Text = BlockDisplayNumber
                Else
                    BlockDisplayNumber = BlockNo
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
                'V4.0.0.0�L��
            End If

            ' ���v�v�Z�\��
            If bDisp = True Then
                If digL = TRIM_MODE_ITTRFT Then
                    TrimData.GetStaticsDataForResistor(BlockNo, gsEDIT_DIGITNUM, ResistorData(RES_DATA_STAT + 1).Label.Text, ResistorData(RES_DATA_STAT + 4).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 1).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 1).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 1).Label.Text, ResistorData(RES_DATA_STAT + 2).Label.Text, ResistorData(RES_DATA_STAT + 5).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 2).Label.Text)
                ElseIf digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS Then
                    TrimData.GetStaticsDataForResistor(BlockNo, gsEDIT_DIGITNUM, cDummy, cDummy, cDummy, cDummy, cDummy, ResistorData(RES_DATA_STAT + 2).Label.Text, ResistorData(RES_DATA_STAT + 5).Label.Text, ResistorData(RES_DATA_STAT + 3 * 2 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 3 + 2).Label.Text, ResistorData(RES_DATA_STAT + 3 * 4 + 2).Label.Text)
                    'V4.0.0.0-44
                    ''ResistorData(RES_DATA_STAT + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 4).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 2 + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 3 + 1).Label.Text = ""
                    ''ResistorData(RES_DATA_STAT + 3 * 4 + 1).Label.Text = ""
                    'V4.0.0.0-44
                End If
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistorDataDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Function

#End Region

#Region "�ڕW�l�A�㉺���l�̕\��"
    '''=========================================================================
    ''' <summary>�ڕW�l�A�㉺���l�̕\��</summary>
    ''' <param name="Target"></param>
    ''' <param name="InitialLo"></param>
    ''' <param name="InitialHi"></param>
    ''' <param name="FinalLo"></param>
    ''' <param name="FinalHi"></param>
    '''=========================================================================
    Public Sub SetTarget(ByVal Target As Double, ByVal InitialLo As Double, ByVal InitialHi As Double, ByVal FinalLo As Double, ByVal FinalHi As Double, ByVal DispTarget As Double)

        '----- V4.0.0.0-31�� -----
        ' SL436S�łȂ����NOP(SL43xR����TrimData�I�u�W�F�N�g���ݒ肳��Ȃ��̂ŉ��L�����s����ƃG���[�ƂȂ�)
        If (giMachineKd <> MACHINE_KD_RS) Then Return
        '----- V4.0.0.0-31�� -----

        '        TrimData.SetTarget(Target, InitialLo, InitialHi, FinalLo, FinalHi, DispTarget)
        TrimData.SetTarget(Target, InitialLo, InitialHi, FinalLo, FinalHi, DispTarget)
        TrimData.GetTarget(StatData(16).Label.Text, StatData(21).Label.Text, StatData(22).Label.Text, StatData(24).Label.Text, StatData(25).Label.Text)
 
    End Sub
#End Region

#Region "�\���f�[�^�X�V"
    ''' <summary>
    ''' ���b�g�X�^�[�g�����i�f�[�^�������j
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TrimmingStart()
        Try
            TrimData.SetResistorCount(typPlateInfo.intResistCntInBlock) ' �P�u���b�N����R����ۑ�����B
            TrimData.LotStart()                                         ' ���b�g�f�[�^�̏�����
            TrimData.PlateStart()                                       ' ��f�[�^�̏�����
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
            bBlockdataDisp = False      '�g���~���O�~�[�h�J�n������A�t���O�𗎂Ƃ��@'V4.0.0.0-62
        Catch ex As Exception
            MsgBox("SimpleTrimmer.TrimmingStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    '----- V4.0.0.0-87�� -----
    Public Sub DspLaserPower()

        Dim LaserPower As Double = 0.0

        Try
            LaserPower = TrimData.GetLaserPower()
            If (LaserPower = 0) Then
                StatData(14).Label.Text = "-.--W"
            Else
                StatData(14).Label.Text = LaserPower.ToString("0.00") + "W"
            End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.DspLaserPower() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    '----- V4.0.0.0-87�� -----

    'V3.1.0.0�D��
    ''' <summary>
    ''' �u���b�N�J�n���̃u���b�N�̓��v�f�[�^�̏���������
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BlockStart()
        Try
            TrimData.BlockStart()                           ' �u���b�N�f�[�^�̏�����
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    'V3.1.0.0�D��
#Region "�P������I�����̃f�[�^����"
    '''=========================================================================
    ''' <summary>�P������I�����̃f�[�^����</summary>
    '''=========================================================================
    Public Sub SetPlateEnd()

        Try
            Call TrimData.SetPlateEnd()

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
            ' DllTrimClassLibrary�Ɉꎞ��~���Ԃ�n��
            TrimData.SetTrimPauseTime(giTrimTimeOpt, StPauseTime.TotalTime)
            '----- V4.11.0.0�C�� -----

            ' "���b�g�ԍ�","�J�n����","�o�ߎ���","�������","����������P�ʎ���","������R��","������R���P�ʎ���","���[�U�p���["�\��
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)

            ' �P�������́A�mBLOCK DATA�n�{�^����\������B
            If TrimData.GetPlateCounter() > 0 Then
                CmdBlockData.Visible = True
            End If

        Catch ex As Exception
            MsgBox("SimpleTrimmer.SetPlateEnd() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

    ''' <summary>
    ''' ���b�g�I������
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetLotEnd()
        Try
            Call TrimData.SetLotEnd()
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SetLotEnd() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' �o�ߎ��Ԃ̍X�V
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ElapsedTimeUpdate()
        Try
            Dim ColmnOff As Integer = 0
            Dim sDummy As String = ""
            Dim strDAT As String = ""                                   ' V4.11.0.0�C

            ' V4.0.0.0-50
            If (IsNothing(TrimData) = True) Then
                Return
            End If

            ' �������͍X�V���Ȃ��B            TrimData.GetRealTimeData(StatData(6).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text)
            '            TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)

            '----- V4.11.0.0�C�� (WALSIN�aSL436S�Ή�) -----
            'TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)
            ' �o�ߎ��Ԃ̕\��
            If (giTrimTimeOpt = 0) Then                                 ' �^�N�g�\�����Ɉꎞ��~���Ԃ�(0=�܂߂�(�W��), 1=�܂߂Ȃ�)
                TrimData.GetRealTimeData(StatData(6).Label.Text, sDummy, sDummy)
            Else
                ' �I�����Ԃ��X�V�����GetRealTimeData��Call����
                TrimData.GetRealTimeData(strDAT, sDummy, sDummy)
                ' �o�ߎ��Ԃ�\������(�ꎞ��~��ʕ\�����͍ŏ��̈�x�̂ݕ\������)
                If (m_blnElapsedTime = True) Then
                    StatData(6).Label.Text = strDAT
                    m_blnElapsedTime = False
                End If
            End If
            '----- V4.11.0.0�C�� -----

            'V4.8.0.1�@��
            '            TrimData.GetYieldData(StatData(0).Label.Text)
            TrimData.GetYieldData(giRateDisp, StatData(0).Label.Text)
            'V4.8.0.1�@��

        Catch ex As Exception
            'V4.11.0.0�A COM�̃I�[�v���G���[��TRAP�ƂȂ�
            'MsgBox("SimpleTrimmer.ElapsedTimeUpdate() TRAP ERROR = " + ex.Message) 
        End Try
    End Sub

    ''' <summary>
    ''' ���v�l�\���f�[�^�̍X�V
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SimpleTrim_LoggingStart()
        Try
            Dim ColmnOff As Integer = 0
            ' �f�W�X�C�b�`�̓ǎ��
            Dim digL As Integer, digH As Integer, digSW As Integer
            Call Form1.GetMoveMode(digL, digH, digSW)

            For Mode As Integer = StatType.PLATE_INITIAL To StatType.LOT_FINAL
                ColmnOff = TKY_DATA_POS + (Mode - StatType.PLATE_INITIAL)
                ' 'V4.0.0.0-44��
                'If ((digL = TRIM_MODE_TRFT Or digL = TRIM_MODE_FT Or digL = TRIM_MODE_MEAS) And (Mode = StatType.PLATE_INITIAL Or Mode = StatType.LOT_INITIAL)) Or digL = TRIM_MODE_POSCHK Or digL = TRIM_MODE_CUT Or digL = TRIM_MODE_STPRPT Then
                '    StatData(ColmnOff).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 2).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 3).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 4).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 5).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 6).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 7).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 8).Label.Text = ""
                '    StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text = ""
                '    Continue For
                'End If
                ' 'V4.0.0.0-44��
                TrimData.GetStaticsData(Mode, gsEDIT_DIGITNUM, StatData(ColmnOff).Label.Text, StatData(ColmnOff + TKY_DATA_COL).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 2).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 3).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 4).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 5).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 6).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 7).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 8).Label.Text, StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text)
                'V5.0.0.4�C�b�o�j��\����
                If giCpk_Disp_Off Then
                    StatData(ColmnOff + TKY_DATA_COL * 9).Label.Text = ""
                End If
                'V5.0.0.4�C��
            Next
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrim_LoggingStart() TRAP ERROR = " + ex.Message)
        End Try
    End Sub

    Public Sub ResistorDataDisplay(ByVal bDisp As Boolean, ByVal BlockNo As Integer, ByVal ResNo As Integer)
        Try
            If Not bBlockdataDisp And bDisp Then    ' ���݃u���b�N�f�[�^���\���ŕ\���w���̎��́A�R�}���h�{�^���������B
                If giAppMode = APP_MODE_IDLE Then 'V4.0.0.0-75
                    Form1.tabCmd.Visible = False
                Else
                    Call Form1.Form1Button(0)
                End If
            End If
            ' �w��̃u���b�N�A��R�ԍ��̃f�[�^��\������A�܂��͏����B
            BlockDisplayNumber = BlockNo
            ResistorDisplayNumber = ResNo
            Call SimpleTrimmer.ResistorDataDisp(bDisp, BlockDisplayNumber, ResistorDisplayNumber)
            If bDisp Then                           ' �\���w���̎��A��ԃt���O��\���ɕύX����B
                bBlockdataDisp = True
            Else
                bBlockdataDisp = False
                Call Form1.Form1Button(1)           ' 
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.ResistorDataDisplay() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "���̑����\�b�h"
    Public Function IsBlockDataDisp() As Boolean
        IsBlockDataDisp = bBlockdataDisp
    End Function

#End Region

#Region "�f�[�^�N���A�{�^������"
    Private Sub DataClrButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataClrButton.Click
        Dim r As Integer

        ' �݌v���N���A���Ă���낵���ł����H
        r = MsgBox(MSG_108, MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal + MsgBoxStyle.MsgBoxSetForeground)
        If (r = MsgBoxResult.Yes) Then

            Debug.WriteLine("�f�o�b�O�E���b�Z�[�W���o��DataClrButton_Click")
            TrimData.ResetLotData()
            TrimData.SetLaserPower(0.0)     'V4.9.0.0�C
            TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
            SimpleTrim_LoggingStart()
            CmdBlockData.Visible = False         ' �mBLOCK DATA�n�{�^�����펞�ɂ���B
            Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
            Call Form1.ClearCounter(1)                              ' ���Y�Ǘ��f�[�^�̃N���A
            Call ClrTrimPrnData()                                   ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) V1.18.0.0�B

            ' ���v�\����ON�̏ꍇ�A�\�����X�V����
            'If Form1.chkDistributeOnOff.Checked = True Then
            gObjFrmDistribute.RedrawGraph() 'V4.0.0.0-81
            'End If
        End If

    End Sub
#End Region

#Region "�f�[�^�ۑ��{�^������"
    Private Sub DataSaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataSaveButton.Click
        Debug.WriteLine("�f�o�b�O�E���b�Z�[�W���o��DataSaveButton_Click")

        'V4.0.0.0-58??
        'Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()
        'TrimData.WriteStaticsData(gSysPrm.stLOG.gsLoggingDir & sFilePath, gsEDIT_DIGITNUM)
        SubDataSave()
        'V4.0.0.0-58??
        If (IsNothing(TrimData) = True) Then Return 'V4.0.0.0-68
        Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()
        'If gSysPrm.stTMN.giMsgTyp = 0 Then
        '    MsgBox("�ۑ������I" & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")
        'Else
        '    MsgBox("Save completion." & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")
        'End If
        MsgBox(SimpleTrimmer_001 & vbCrLf & " (" & gSysPrm.stLOG.gsLoggingDir & sFilePath & ")")

    End Sub

    ''' <summary>
    ''' �f�[�^�ۑ�         'V4.0.0.0-58??
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SubDataSave()

        If (IsNothing(TrimData) = True) Then Return 'V4.0.0.0-68
        Dim sFilePath As String = TrimData.GetStaticsDataLogFileName()

        TrimData.WriteStaticsData(gSysPrm.stLOG.gsLoggingDir & sFilePath, gsEDIT_DIGITNUM)

    End Sub
#End Region

#Region "�a�k�n�b�j �c�`�s�`�{�^������"
    Public Sub PlateTrimmingEnd()
        Try
            If TrimData.GetPlateCounter() > 0 Then
                CmdBlockData.Visible = True         ' �P�������́A�mBLOCK DATA�n�{�^����\������B
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockDataButtonDisp() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Private Sub cmdBlockData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdBlockData.Click
        Debug.WriteLine("�f�o�b�O�E���b�Z�[�W���o��cmdBlockData_Click")
        BlockDisplayNumber = 1      'V4.0.0.0�L
        Call ResistorDataDisplay(True, BlockDisplayNumber, ResistorDisplayNumber)
    End Sub
#End Region

#Region "�R�}���h�{�^������"
    Public Sub CommandButtonTutorial()
        Try
            If gCmpTrimDataFlg = 1 Then ' �Z�[�u�R�}���h �ҏW���̃f�[�^����
                commandtutorial.SetDataChange()
            Else
                commandtutorial.SetDataSave()
            End If

            Form1.CmdLoad.BackColor = commandtutorial.GetLoadColor()

            Form1.CmdSave.BackColor = commandtutorial.GetSaveColor()

            Form1.CmdTeach.BackColor = commandtutorial.GetTeachColor()


        Catch ex As Exception
            MsgBox("SimpleTrimmer.CommandButtonChange() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#Region "�V���v���g���}�ǉ��{�^������"
    ''' <summary>
    ''' �����e�i���X�{�^������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdMainte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdMainte.Click
        Dim r As Integer
        'V6.0.0.0�Q        Dim frmobject As Object
        Dim frmobject As frmMaintenance 'V6.0.0.0�Q

        ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��
        r = Form1.TrimStateOn(F_MAINTENANCE, APP_MODE_MAINTENANCE, "", "")
        If (r <> cFRS_NORMAL) Then Return

        frmobject = New frmMaintenance
        frmobject.Showdialog()
        'V5.0.0.1�R��
        If (frmobject Is Nothing = False) Then
            Call frmobject.Close()                                        ' �I�u�W�F�N�g�J��
            Call frmobject.Dispose()                                      ' ���\�[�X�J��
            frmobject = Nothing
        End If
        'V5.0.0.1�R��

        ' �I������
        Call Form1.TrimStateOff()                                         ' �g���}���u��Ԃ𓮍쒆�ɐݒ肷��

    End Sub


    ''' <summary>
    ''' �v���[�u�N���[�j���O�{�^������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdProbeClean_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdProbeClean.Click
        Dim r As Integer

        ' �R�}���h���s
        r = Form1.CmdExec_Proc(F_PROBE_CLEANING, APP_MODE_PROBE_CLEANING, MSG_OPLOG_MAINT, "")

    End Sub

#Region "BP Offset �����{�^������"
    '''=========================================================================
    ''' <summary>BP Offset �����{�^������</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BPAdjustButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BPAdjustButton.Click

        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer

        '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
        ' �g���~���O�J�n�u���b�N�ԍ����\���Ƃ���
        Form1.GrpStartBlk.Visible = False
        '----- V4.11.0.0�D�� -----

        '�f�W�^���X�C�b�`�̒l�擾
        Call Form1.GetMoveMode(digL, digH, digSW)

        Form1.TimerAdjust.Enabled = False

        gObjADJ = New frmFineAdjust()

        SetTeachVideoSize()                                     'V2.0.0.0�Q
        Form1.Instance.VideoLibrary1.SetTrackBarVisible(True)   'V6.0.0.0�E

        'V4.0.0.0�Q
        Call SimpleTrimmer.ResistorDataDisp(False, 0, 0)
        'V4.0.0.0�Q

        ' �f�[�^�\�����\���ɂ���
        Call Form1.SetDataDisplayOff()                          'V4.0.0.0�J
        GroupBoxVisibleChange(False)                            'V4.0.0.0�J

        '#4.12.2.0�E        Call gObjADJ.SetInitialData(gSysPrm, digL, digH, gCurPlateNo, gCurBlockNo)
        gObjADJ.SetInitialData(gSysPrm, digL, digH, gCurPlateNo, gCurBlockNo,
                               gCurPlateNoX, gCurPlateNoY, gCurBlockNoX, gCurBlockNoY)  '#4.12.2.0�E
        Call gObjADJ.Focus()
        Call gObjADJ.Show()

    End Sub
#End Region

    ''' <summary>
    ''' Lot�ԍ����͉�ʂ̕\�� 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdLotNumber_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdLotNumber.Click
        'V6.0.0.0�O        gObjADJ = New frmLotNoInput
        'V6.0.0.0�O        Call gObjADJ.Focus()
        'V6.0.0.0�O        Call gObjADJ.Show()
        Using frm As New frmLotNoInput()        'V6.0.0.0�O
            frm.ShowDialog(Form1.Instance)
        End Using
    End Sub

#End Region

#Region "Video�T�C�Y�̕ύX"

    ''' <summary>
    ''' �V���v���g���}�p��Video�T�C�Y�ɕύX���� 'V2.0.0.0�Q
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetSimpleVideoSize()
        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        '        Form1.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        'V4.0.0.0�I        Form1.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_SIZE, SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        Form1.Instance.VideoLibrary1.SetVideoSizeAndCross(SIMPLE_SIZE, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY, CROSS_LINEX, CROSS_LINEY)
        'Form1.VideoLibrary1.Width = SIMPLE_PICTURE_SIZEX
        'Form1.VideoLibrary1.Height = SIMPLE_PICTURE_SIZEY
        Form1.Instance.VideoLibrary1.Size = New Size(SIMPLE_PICTURE_SIZEX, SIMPLE_PICTURE_SIZEY)
#If False Then              'V6.0.0.0�C
        Form1.Picture1.Top = CROSS_LINEY + Form1.VideoLibrary1.Top
        Form1.Picture2.Left = CROSS_LINEX + Form1.VideoLibrary1.Left

        'Form1.Picture1.Width = SIMPLE_PICTURE_SIZEX / 15
        'Form1.Picture2.Height = SIMPLE_PICTURE_SIZEY / 15
        Form1.Picture1.Width = SIMPLE_PICTURE_SIZEX
        Form1.Picture2.Height = SIMPLE_PICTURE_SIZEY
#Else
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(CROSS_LINEX, CROSS_LINEY)
#End If
        Form1.Instance.PanelGraphOnOff(True)                        ' V4.0.0.0�K

    End Sub

    ''' <summary>
    ''' �e�B�[�`���O�p�ɏ]����Video�T�C�Y�ɕύX���� 'V2.0.0.0�Q
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetTeachVideoSize()
        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        '        Form1.VideoLibrary1.SetVideoSizeAndCross(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
        Form1.Instance.VideoLibrary1.SetVideoSizeAndCross(NORMAL_SIZE, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY, gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
        'Form1.Instance.VideoLibrary1.Width = NORMAL_PICTURE_SIZEX
        'Form1.Instance.VideoLibrary1.Height = NORMAL_PICTURE_SIZEY
        Form1.Instance.VideoLibrary1.Size = New Size(NORMAL_PICTURE_SIZEX, NORMAL_PICTURE_SIZEY)
#If False Then              'V6.0.0.0�C
        Form1.Picture1.Top = gSysPrm.stDVR.giCrossLineX + Form1.VideoLibrary1.Top
        Form1.Picture2.Left = gSysPrm.stDVR.giCrossLineY + Form1.VideoLibrary1.Left

        Form1.Picture1.Width = NORMAL_PICTURE_SIZEX
        Form1.Picture2.Height = NORMAL_PICTURE_SIZEY
#Else
        ' Top, Left �� X, Y ���t�ɂȂ��Ă���
        Form1.Instance.VideoLibrary1.SetCrossLineCenter(gSysPrm.stDVR.giCrossLineY, gSysPrm.stDVR.giCrossLineX)
#End If
        Form1.Instance.PanelGraphOnOff(False)                        ' V4.0.0.0�K

    End Sub
#End Region

#Region "�����O���[�v�{�b�N�X�̕\���A��\����؂�ւ���"
    '''=========================================================================
    '''<summary>�����O���[�v�{�b�N�X�̕\���A��\����؂�ւ���</summary>
    '''<param name="VisibleFlag"> (IN)VisibleFlag</param>
    '''=========================================================================
    Public Sub GroupBoxVisibleChange(ByVal VisibleFlag As Boolean)

        Form1.GrpMode.Visible = VisibleFlag

    End Sub

#End Region

#Region "�����O���[�v�{�b�N�X�̗L���A������؂�ւ���"
    '''=========================================================================
    '''<summary>�����O���[�v�{�b�N�X�̗L���A������؂�ւ���</summary>
    '''<param name="EnableFlag"> (IN)EnableFlag</param>
    '''=========================================================================
    Public Sub GroupBoxEnableChange(ByVal EnableFlag As Boolean)

        'V4.0.0.0-55
        '        Form1.GrpMode.Enabled = EnableFlag
        '�ꎞ��~�p��ADJ�͏�ɉ�����K�v�����邽�߁A�ʂɐ؂�ւ���    
        '        BPAdjustButton.Enabled = True 'V4.11.0.0�O
        BPAdjustButton.Enabled = EnableFlag 'V4.11.0.0�O

        DataClrButton.Enabled = EnableFlag
        DataSaveButton.Enabled = EnableFlag
        Form1.CbDigSwH.Enabled = EnableFlag
        Form1.CbDigSwL.Enabled = EnableFlag
        'V4.0.0.0-55
        'V4.0.0.0-82        ' 
        BlockNextButton.Enabled = EnableFlag
        BlockRvsButton.Enabled = EnableFlag
        BlockMainButton.Enabled = EnableFlag
        'V4.0.0.0-82        ' 
        DataEditButton.Enabled = EnableFlag 'V4.0.0.0-84 

    End Sub

#End Region
    ' ������ V3.1.0.0�C 2014/12/05
#Region "��R�l�͈̔̓`�F�b�N"
    ''' <summary>
    ''' ��R�l�͈̔̓`�F�b�N
    ''' </summary>
    ''' <param name="nType">��ʁi0=�����A1=�ŏI�j</param>
    ''' <param name="strValue">�f�[�^������[xxx.xxxxxxx(x.xxx)]</param>
    ''' <returns>True=�͈͓��AFalse=�͈͊O</returns>
    ''' <remarks></remarks>
    Private Function IsResistorRange(ByVal nType As Integer, ByVal strValue As String) As Boolean
        Dim dTmp As Double
        Dim dMin As Double
        Dim dMax As Double
        Dim dTarget As Double
        Dim dValue As Double
        '----- V4.0.0.0�S�� -----
        Dim i As Integer
        Dim nIndex As Integer
        Dim bRet As Boolean
        ' UNDONE: TrimData.GetResDispData�̏������ύX���K�v
        'Dim strUnit() As String = {"M��", "K��", "��", "m��"}   ' �P��
        Dim strUnit() As String = {SimpleTrimmer_040, SimpleTrimmer_041, SimpleTrimmer_042, SimpleTrimmer_043}   ' �P��
        '----- V4.0.0.0�S�� -----
        Dim tempTarget As String
        IsResistorRange = False

        Do
            '----- V4.0.0.0�S�� -----
            '' �f�[�^�����񂩂��R�l���擾
            'If Double.TryParse(strValue.Substring(0, strValue.IndexOf("(")), dValue) = False Then
            '    System.Diagnostics.Debug.WriteLine("�f�[�^�����񂩂��R�l�̎擾�Ɏ��s���܂����B{0}", strValue)
            '    Exit Do
            'End If

            '' �ڕW�l
            'If Double.TryParse(StatData(16).Label.Text.Substring(0, StatData(16).Label.Text.Length - 1), dTarget) = False Then
            '    System.Diagnostics.Debug.WriteLine("�ڕW�l�̕ϊ��Ɏ��s���܂����B{0}", StatData(16).Label.Text)
            '    Exit Do
            'End If

            ' �f�[�^�����񂩂��R�l���擾
            bRet = False
            For i = 0 To strUnit.Length - 1 Step 1
                nIndex = strValue.IndexOf(strUnit(i))
                If nIndex < 0 Then
                    Continue For
                End If

                bRet = Double.TryParse(strValue.Substring(0, nIndex), dValue)
                If bRet = True Then
                    If i = 0 Then
                        dValue = dValue * 1000000.0
                    ElseIf i = 1 Then
                        dValue = dValue * 1000.0
                    ElseIf i = 2 Then
                        dValue = dValue * 1.0
                    ElseIf i = 3 Then
                        dValue = dValue / 1000.0
                    End If
                    Exit For
                End If
            Next i

            If bRet = False Then
                System.Diagnostics.Debug.WriteLine("�f�[�^�����񂩂��R�l�̎擾�Ɏ��s���܂����B{0}", strValue)
            End If

            ' �ڕW�l
            bRet = False
            For i = 0 To strUnit.Length - 1 Step 1
                'V4.11.0.0�N��
                If (giTargetOfs = 1) Then
                    tempTarget = TrimData.GetChangeOhmUnit(typResistorInfoArray(1).dblTrimTargetVal)
                    nIndex = tempTarget.IndexOf(strUnit(i))
                    If nIndex < 0 Then
                        Continue For
                    End If
                Else
                    nIndex = StatData(16).Label.Text.IndexOf(strUnit(i))
                    If nIndex < 0 Then
                        Continue For
                    End If
                    tempTarget = StatData(16).Label.Text
                End If

                bRet = Double.TryParse(tempTarget.Substring(0, tempTarget.IndexOf(strUnit(i))), dTarget)
                'V4.11.0.0�N��
                If bRet = True Then
                    If i = 0 Then
                        dTarget = dTarget * 1000000.0
                    ElseIf i = 1 Then
                        dTarget = dTarget * 1000.0
                    ElseIf i = 2 Then
                        dTarget = dTarget * 1.0
                    ElseIf i = 3 Then
                        dTarget = dTarget / 1000.0
                    End If
                    Exit For
                End If
            Next i

            If bRet = False Then
                System.Diagnostics.Debug.WriteLine("�ڕW�l�̕ϊ��Ɏ��s���܂����B{0}", StatData(16).Label.Text)
                Exit Do
            End If
            '----- V4.0.0.0�S�� -----

            If nType = 0 Then   ' �C�j�V�����e�X�g
                If Double.TryParse(StatData(21).Label.Text.Substring(0, StatData(21).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("�C�j�V�����e�X�g�̉����l�̕ϊ��Ɏ��s���܂����B{0}", StatData(21).Label.Text)
                    dTmp = 0.0
                End If

                dMin = dTarget + (dTarget * (dTmp / 100.0))

                If Double.TryParse(StatData(22).Label.Text.Substring(0, StatData(22).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("�C�j�V�����e�X�g�̏���l�̕ϊ��Ɏ��s���܂����B{0}", StatData(22).Label.Text)
                    dTmp = 0.0
                End If

                dMax = dTarget + (dTarget * (dTmp / 100.0))

            Else                ' �t�@�C�i���e�X�g
                If Double.TryParse(StatData(24).Label.Text.Substring(0, StatData(24).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("�t�@�C�i���e�X�g�̉����l�̕ϊ��Ɏ��s���܂����B{0}", StatData(24).Label.Text)
                    dTmp = 0.0
                End If

                dMin = dTarget + (dTarget * (dTmp / 100.0))

                If Double.TryParse(StatData(25).Label.Text.Substring(0, StatData(25).Label.Text.Length - 1), dTmp) = False Then
                    System.Diagnostics.Debug.WriteLine("�t�@�C�i���e�X�g�̏���l�̕ϊ��Ɏ��s���܂����B{0}", StatData(25).Label.Text)
                    dTmp = 0.0
                End If

                dMax = dTarget + (dTarget * (dTmp / 100.0))

            End If

            ' �͈̓`�F�b�N
            'System.Diagnostics.Debug.WriteLine("�͈̓`�F�b�N({0}) {1}��{2}��{3}", nType, dMin, dValue, dMax)
            If (dMin <= dValue) And (dValue <= dMax) Then
                IsResistorRange = True
            End If

        Loop While False

    End Function
#End Region
    ' ������ V3.1.0.0�C 2014/12/05
#Region "�V���v���g���}�O���t�\���p�t�H�[��������"
    '''=========================================================================
    '''<summary>�V���v���g���}�O���t�\���p�t�H�[��������</summary>
    '''<remarks></remarks>
    '''=========================================================================
    Private Sub InitializeFormGraphPanel()
        Dim strMSG As String

        Try
            ' ���z�}�\���p���x���z��̏�����
            gDistRegNumLblAry(0) = Form1.LblRegN_00              ' ���z�O���t��R���z��(0�`11)
            gDistRegNumLblAry(1) = Form1.LblRegN_01
            gDistRegNumLblAry(2) = Form1.LblRegN_02
            gDistRegNumLblAry(3) = Form1.LblRegN_03
            gDistRegNumLblAry(4) = Form1.LblRegN_04
            gDistRegNumLblAry(5) = Form1.LblRegN_05
            gDistRegNumLblAry(6) = Form1.LblRegN_06
            gDistRegNumLblAry(7) = Form1.LblRegN_07
            gDistRegNumLblAry(8) = Form1.LblRegN_08
            gDistRegNumLblAry(9) = Form1.LblRegN_09
            gDistRegNumLblAry(10) = Form1.LblRegN_10
            gDistRegNumLblAry(11) = Form1.LblRegN_11

            gDistGrpPerLblAry(0) = Form1.LblGrpPer_00           ' ���z�O���t%�z��(0�`11)
            gDistGrpPerLblAry(1) = Form1.LblGrpPer_01
            gDistGrpPerLblAry(2) = Form1.LblGrpPer_02
            gDistGrpPerLblAry(3) = Form1.LblGrpPer_03
            gDistGrpPerLblAry(4) = Form1.LblGrpPer_04
            gDistGrpPerLblAry(5) = Form1.LblGrpPer_05
            gDistGrpPerLblAry(6) = Form1.LblGrpPer_06
            gDistGrpPerLblAry(7) = Form1.LblGrpPer_07
            gDistGrpPerLblAry(8) = Form1.LblGrpPer_08
            gDistGrpPerLblAry(9) = Form1.LblGrpPer_09
            gDistGrpPerLblAry(10) = Form1.LblGrpPer_10
            gDistGrpPerLblAry(11) = Form1.LblGrpPer_11

            gDistShpGrpLblAry(0) = Form1.LblShpGrp_00                      ' ���z�O���t�z��(0�`11)
            gDistShpGrpLblAry(1) = Form1.LblShpGrp_01
            gDistShpGrpLblAry(2) = Form1.LblShpGrp_02
            gDistShpGrpLblAry(3) = Form1.LblShpGrp_03
            gDistShpGrpLblAry(4) = Form1.LblShpGrp_04
            gDistShpGrpLblAry(5) = Form1.LblShpGrp_05
            gDistShpGrpLblAry(6) = Form1.LblShpGrp_06
            gDistShpGrpLblAry(7) = Form1.LblShpGrp_07
            gDistShpGrpLblAry(8) = Form1.LblShpGrp_08
            gDistShpGrpLblAry(9) = Form1.LblShpGrp_09
            gDistShpGrpLblAry(10) = Form1.LblShpGrp_10
            gDistShpGrpLblAry(11) = Form1.LblShpGrp_11

            'V4.0.0.0�K��
            gGoodChip = Form1.lblGoodChip2
            gNgChip = Form1.lblNgChip2
            gMaxValue = Form1.lblMaxValue2
            gMinValue = Form1.lblMinValue2
            gAverageValue = Form1.lblAverageValue2
            gDeviationValue = Form1.lblDeviationValue2
            gGraphAccumulationTitle = Form1.lblGraphAccumulationTitle2
            gRegistUnit = Form1.lblRegistUnit2
            'V4.0.0.0�K��


            'DistRegItLblAry(i) = New System.Windows.Forms.Label     ' ���z�O���t��R��(IT)�z��
            'DistRegFtLblAry(i) = New System.Windows.Forms.Label     ' ���z�O���t��R��(FT)�z��

            ' �g���b�v�G���[������ 
        Catch ex As Exception
            strMSG = "frmDistribution.InitializeForm() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "���u���b�N�f�[�^�{�^�����������Ƃ��̏���"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0�L
    ''' ���u���b�N�f�[�^�{�^�����������Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockNextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockNextButton.Click
        Try
#If False Then                          'V5.0.0.9�O
            If (typPlateInfo.intResistDir = 0) Then                     ' ��R(����)���ѕ���(0:X, 1:Y)
                If typPlateInfo.intBlockCntYDir > BlockDisplayNumber Then
                    BlockDisplayNumber = BlockDisplayNumber + 1
                    Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
            Else
                If typPlateInfo.intBlockCntXDir > BlockDisplayNumber Then
                    BlockDisplayNumber = BlockDisplayNumber + 1
                    Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                    BlockNoLabel.Text = BlockDisplayNumber
                End If
            End If
#Else
            If ((typPlateInfo.intBlockCntXDir * typPlateInfo.intBlockCntYDir) > BlockDisplayNumber) Then
                BlockDisplayNumber = BlockDisplayNumber + 1
                SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
                BlockNoLabel.Text = BlockDisplayNumber
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.BlockNextButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub

#End Region

#Region "�O�u���b�N�f�[�^�{�^�����������Ƃ��̏���"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0�L
    ''' �O�u���b�N�f�[�^�{�^�����������Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockRvsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockRvsButton.Click
        If BlockDisplayNumber > 1 Then
            BlockDisplayNumber = BlockDisplayNumber - 1
            Call SimpleTrimmer.ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
            BlockNoLabel.Text = BlockDisplayNumber
        End If
    End Sub
#End Region

#Region "���x����BlockNo���������Ƃ��̏���"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0�L
    ''' ���x����BlockNo���������Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub BlockNoLabel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockNoLabel.Click

    End Sub
#End Region

#Region "���x���ɕ\������BlockNo�̍X�V"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0�L
    ''' ���x���ɕ\������BlockNo�̍X�V
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetBlockDisplayNumber(ByVal BlockNo As Integer,
                                     Optional ByVal doResistorDataDisp As Boolean = False) 'V5.0.0.9�O
        'V5.0.0.9�O    Public Sub SetBlockDisplayNumber(ByVal BlockNo As Integer)

        BlockDisplayNumber = BlockNo

        'V5.0.0.9�O                  ��
        If (doResistorDataDisp) AndAlso (BlockNoLabel.Visible) Then
            ' �Y���u���b�N�̒�R�f�[�^��\������
            ResistorDataDisp(True, BlockDisplayNumber, ResistorDisplayNumber)
            BlockNoLabel.Text = BlockDisplayNumber
        End If
        'V5.0.0.9�O                  ��
    End Sub

    '''=========================================================================
    ''' <summary>    'V4.3.0.0�@
    ''' BlockNo�̎擾
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub GetBlockDisplayNumber(ByRef BlockNo As Integer)

        BlockNo = TrimData.GetBlockNumber()

    End Sub
#End Region

#Region "Lotno�\���̍X�V"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0-42
    ''' ���x���ɕ\������LotNo�̍X�V
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub UpdateLotNo(ByVal Lotno As String)

        StatData(2).Label.Text = Lotno

    End Sub
#End Region

#Region "����Lot�̂̎擾"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0-42
    ''' ����Lot�̂̎擾 
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub GetLotNo(ByRef Lotno As String)

        TrimData.GetLotNo(Lotno)

    End Sub
#End Region

    'V4.0.0.0-76
    Private Sub BlockMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlockMainButton.Click
        'IDLE���̂ݔ�������悤�ɕύX����
        If giAppMode = APP_MODE_IDLE Then 'V4.0.0.0-75
            Call SimpleTrimmer.ResistorDataDisplay(False, Integer.Parse(BlockDisplayNumber), 1)
            FrmBlockDisp.Close()
        End If
    End Sub

    ''' <summary>
    ''' ���C����ʂ̈ꎞ��~�{�^������f�[�^�ҏW��ʂ̋N�� 'V4.0.0.0-84
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataEditStart() As Integer

        Dim r As Integer
        Try
            ' �f�[�^�ҏW�v���O�������N������(�ꎞ��~���[�h)
            r = Form1.ExecEditProgram(1)                                ' ����~���̃G���[��������APP�I������̂Ŗ߂��Ă��Ȃ� 
            Call ZCONRST()                                              ' �R���\�[���L�[���b�`���� ###226
            If (r <> cFRS_NORMAL) Then
                Return r                                            ' �ꎞ��~��ʏ����I����
            End If

            ' �v���[�g�̃X�^�[�g�|�W�V�����ݒ�                          ' ###079 File_Read()��Call�����gBlkStagePosX,Y()���N���A�����̂ōĐݒ肷��
            Call CalcPlateXYStartPos()
            ' �u���b�N�̃X�^�[�g�|�W�V�����ݒ�                          ' ###079
            Call CalcBlockXYStartPos()

            ' �g���~���O�f�[�^��INtime���ɑ��M���� ###087
            Call TRIMEND()                                              ' INtime���̃��������
            '----- ###257�� -----
            ' FL�����猻�݂̉��H��������M����
            r = TrimCondInfoRcv(stCND)
            If (r <> SerialErrorCode.rRS_OK) Then
                ' "�e�k�����H�����̃��[�h�Ɏ��s���܂����B"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSG_141, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                Return cFRS_COM_FL_ERR
            End If
            '----- ###257�� -----
            r = SendTrimData()                                          ' �g���~���O�f�[�^��INtime���ɑ��M����
            If (r <> cFRS_NORMAL) Then
                ' "�g���~���O�f�[�^�̐ݒ�Ɏ��s���܂����B" & vbCrLf & "�g���~���O�f�[�^�ɖ�肪�Ȃ����m�F���Ă��������B"
                Call Form1.System1.TrmMsgBox(gSysPrm, MSGERR_SEND_TRIMDATA, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, TITLE_4)
                Return cFRS_FIOERR_INP
            End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SimpleTrimmerInit() TRAP ERROR = " + ex.Message)
        End Try


    End Function

    ''' <summary>
    ''' �ꎞ��~��ʂŁA�f�[�^�ҏW�{�^�����������Ƃ��̏��� 'V4.0.0.0-84
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DataEditButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataEditButton.Click

        DataEditStart()

    End Sub

    ''' <summary>
    ''' �R�}���h�{�^���̐ؑւ�
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CommandEnableSet(ByVal flg As Boolean) As Integer

        If gKeiTyp <> KEY_TYPE_RS Then
            Return 0
        End If

        Form1.tabCmd.Enabled = flg

        Return 0

    End Function

    ''' <summary>
    ''' ���[�U�I�t���`�F�b�N���� 'V4.0.0.0-86
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLaserOffIO(modecheck As Boolean) As Integer 'V5.0.0.1�K
        '    Public Function GetLaserOffIO() As Integer
        Dim r As Integer
        Dim bit As Integer
        Dim Adr As Integer
        Dim Dat As Integer
        Dim strMsg As String
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        Dim ChkExec As Boolean 'V5.0.0.1�K

        'V5.0.0.1�K��
        ChkExec = True
        If (gKeiTyp = KEY_TYPE_RS) Then
            If modecheck = True Then
                Call Form1.GetMoveMode(digL, digH, digSW)
                If ((digL = 0) Or (digL = 1) Or (digL = 5)) Then
                    ChkExec = True
                Else
                    ChkExec = False
                End If
            End If
            If ChkExec = True Then
                'V5.0.0.1�K��
                Adr = &H2102
                bit = &H10
                r = INP16(Adr, Dat)        ' // ' // �f�[�^���[�h
                If (bit And Dat) Then
                    If (IsNothing(gObjADJ) = False) Then
                        gObjADJ.TopMost = False
                    End If
                    strMsg = SimpleTrimmer_039 ' Laser��ON���Ă�������
                    'V5.0.0.1�H��
                    '                    MsgBox(strMsg)
                    MsgBox(strMsg, MsgBoxStyle.OkOnly + MsgBoxStyle.MsgBoxSetForeground)
                    'MessageBox.Show(strMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    '                    MessageBox.Show(Form1, strMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    If (IsNothing(gObjADJ) = False) Then
                        gObjADJ.TopMost = True
                    End If
                    'V5.0.0.1�H��
                    Return 1
                End If
            End If
        End If

        Return 0

    End Function

    ''' <summary>
    ''' �v���[�u�N���[�j���O�{�^����Visible�ݒ���s��
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ProbeBtnVisibleSet(ByVal mode As Boolean)

        If IsNothing(CmdProbeClean) = False Then
            CmdProbeClean.Visible = mode
        End If

    End Sub

    ''' <summary>
    ''' �v���[�u�N���[�j���O�{�^����Enable�ݒ���s��
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub ProbeBtnEnableSet(ByVal mode As Boolean)

        If IsNothing(CmdProbeClean) = False Then
            CmdProbeClean.Enabled = mode
        End If

    End Sub

    ''' <summary>
    ''' I/O�m�F�{�^����Visible�ݒ���s��
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub MaintBtnVisibleSet(ByVal mode As Boolean)
        If IsNothing(CmdMainte) = False Then
            CmdMainte.Visible = mode
        End If
    End Sub

    ''' <summary>
    ''' I/O�m�F�{�^����Enable�ݒ���s��
    ''' </summary>
    ''' <param name="mode"></param>
    ''' <remarks></remarks>
    Public Sub MaintBtnEnableSet(ByVal mode As Boolean)
        If IsNothing(CmdMainte) = False Then
            CmdMainte.Enabled = mode

        End If

    End Sub

    ''' <summary>
    ''' �u���b�N�ԍ��\���̐ؑւ�
    ''' </summary>
    ''' <param name="flg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BlockNoBtnVisible(ByVal flg As Boolean) As Integer

        If gKeiTyp <> KEY_TYPE_RS Then
            Return 0
        End If

        '----- V4.11.0.0�D�� (WALSIN�aSL436S�Ή�) -----
        ' �g���~���O�J�n�u���b�N�ԍ��w��̗L���̏ꍇ�̓u���b�N���͕\�����Ȃ�
        'NowBlockNoLabel.Visible = flg
        If (giStartBlkAss = 0) Then
            NowBlockNoLabel.Visible = flg
        Else
            NowBlockNoLabel.Visible = False
        End If
        '----- V4.11.0.0�D�� -----

        Return 0

    End Function

#Region "���x���ɕ\������BlockNo�̍X�V"
    '''=========================================================================
    ''' <summary>    'V4.0.0.0�L
    ''' ���x���ɕ\������BlockNo�̍X�V
    ''' </summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub SetNowBlockDspNum(ByVal BlockNo As Integer)

        If gKeiTyp <> KEY_TYPE_RS Then
            Return
        End If

        NowBlockNoLabel.Text = "BlockNo:" + BlockNo.ToString() 'V4.2.0.0�@

    End Sub
#End Region

#Region "���Y�Ǘ����̃N���A"
    '''=========================================================================
    ''' <summary>���Y�Ǘ����̃N���A V6.0.3.0�H</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Sub ClearDispData()

        Debug.WriteLine("�f�o�b�O�E���b�Z�[�W���o��DataClrButton_Click")
        TrimData.ResetLotData()
        TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
        SimpleTrim_LoggingStart()
        CmdBlockData.Visible = False                                ' �mBLOCK DATA�n�{�^�����펞�ɂ���B
        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
        Call Form1.ClearCounter(1)                                  ' ���Y�Ǘ��f�[�^�̃N���A
        Call ClrTrimPrnData()                                       ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) V1.18.0.0�B

        ' ���v�\����ON�̏ꍇ�A�\�����X�V����
        'If Form1.chkDistributeOnOff.Checked = True Then
        gObjFrmDistribute.RedrawGraph()                             ' V4.0.0.0-81

    End Sub
#End Region

    'V4.8.0.1�@��
    ''' <summary>
    ''' �s�Ǘ��\����ύX�����Ƃ��̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CheckNGRate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckNGRate.CheckedChanged

        If CheckNGRate.Checked = True Then
            giRateDisp = 1
        Else
            giRateDisp = 0
        End If
        TrimData.GetYieldData(giRateDisp, StatData(0).Label.Text)

    End Sub
    'V4.8.0.1�@��

    ''' <summary>
    ''' ��ʂɕ\������Ă���Hi�ALo�AOpen�̐���Ԃ�  'V4.9.0.0�@
    ''' </summary>
    ''' <param name="LotPlate"></param>     // 0:���b�g�A1:���
    ''' <param name="HiLoOpen"></param>     // 0:Lo-NG , 1:Hi-NG , 2:Open-NG
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHiLoOpenCount(ByVal LotPlate As Integer, ByVal HiLoOpen As Integer, ByRef It As Double, ByRef Ft As Double) As Integer
        Dim strITCount As String
        Dim strFTCount As String

        GetHiLoOpenCount = 0
        If (HiLoOpen = UNIT_LO_NG) Then  ' Lo-NG

            If (LotPlate = UNIT_PLATE) Then
                ' ��F��������A�ŏI����FLow-NG
                strITCount = StatData(45).Label.Text
                strFTCount = StatData(46).Label.Text
            Else
                ' Lot�F��������A�ŏI����FLow-NG
                strITCount = StatData(47).Label.Text
                strFTCount = StatData(48).Label.Text
            End If

        ElseIf (HiLoOpen = UNIT_HI_NG) Then  ' Hi-NG 

            If (LotPlate = UNIT_PLATE) Then
                ' ��F��������A�ŏI����FHigh-NG
                strITCount = StatData(50).Label.Text
                strFTCount = StatData(51).Label.Text
            Else
                ' Lot�F��������A�ŏI����FHigh-NG
                strITCount = StatData(52).Label.Text
                strFTCount = StatData(53).Label.Text
            End If

        Else    'UNIT_OPEN_NG

            If (LotPlate = UNIT_PLATE) Then
                ' ��F��������A�ŏI����FOpen-NG:RangeOver
                strITCount = StatData(55).Label.Text
                strFTCount = StatData(56).Label.Text
            Else
                ' Lot�F��������A�ŏI����FOpen-NG:RangeOver
                strITCount = StatData(57).Label.Text
                strFTCount = StatData(58).Label.Text
            End If

        End If

        If Trim(strITCount) <> "" Then
            It = Val(strITCount)
        Else
            It = 0
            GetHiLoOpenCount = 1
        End If

        If Trim(strFTCount) <> "" Then
            Ft = Val(strFTCount)
        Else
            Ft = 0
            GetHiLoOpenCount = 1
        End If

    End Function

    ''' <summary>
    ''' NG�������烍�b�g�̒��f�𔻒f����  'V4.9.0.0�@
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function JudgeLotStop() As Integer
        Dim IT As Double
        Dim FT As Double
        Dim strYield As String
        ' V5.0.0.1-30��
        Dim digL As Integer
        Dim digH As Integer
        Dim digSW As Integer
        ' V5.0.0.1-30��

        JudgeLotStop = cFRS_NORMAL
        strYield = ""

        ' V5.0.0.1-30��
        Call Form1.GetMoveMode(digL, digH, digSW)           ' �f�W�X�C�b�`�̓ǎ��
        If (digL <> 0) And (digL <> 1) And (digL <> 2) Then
            Return JudgeLotStop
        End If
        ' V5.0.0.1-30��

        ' Yield����
        If JudgeNgRate.CheckYeld = True Then
            TrimData.GetYieldData(99, strYield)
            If strYield <> "" Then

                If Double.Parse(strYield) <= JudgeNgRate.ValYield Then
                    JudgeLotStop = 1
                    Return JudgeLotStop
                End If
            End If
        End If

        ' OverRange����
        If JudgeNgRate.CheckOverRange = True Then
            GetHiLoOpenCount(UNIT_PLATE, UNIT_OPEN_NG, IT, FT)
            If IT >= JudgeNgRate.ValOverRange Then
                JudgeLotStop = 2
                Return JudgeLotStop
            End If
        End If

        ' IT-HI����
        If JudgeNgRate.CheckITHI = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block�P�ʂ̏W�v�Ŕ���
            '    JudgeLotStop = 3
            '    Return JudgeLotStop

            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_PLATE, UNIT_HI_NG, IT, FT)
                If IT >= JudgeNgRate.ValITHI Then
                    JudgeLotStop = 4
                    Return JudgeLotStop
                End If
            Else
                ' LOT�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_LOT, UNIT_HI_NG, IT, FT)
                If IT >= JudgeNgRate.ValITHI Then
                    JudgeLotStop = 5
                    Return JudgeLotStop
                End If

            End If
        End If

        ' IT-LO����
        If JudgeNgRate.CheckITLO = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block�P�ʂ̏W�v�Ŕ���
            '    JudgeLotStop = 6
            '    Return JudgeLotStop

            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_PLATE, UNIT_LO_NG, IT, FT)
                If IT >= JudgeNgRate.ValITLO Then
                    JudgeLotStop = 7
                    Return JudgeLotStop
                End If
            Else
                ' LOT�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_LOT, UNIT_LO_NG, IT, FT)
                If IT >= JudgeNgRate.ValITLO Then
                    JudgeLotStop = 8
                    Return JudgeLotStop
                End If

            End If
        End If

        ' FT-HI����
        If JudgeNgRate.CheckFTHI = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block�P�ʂ̏W�v�Ŕ���
            '    JudgeLotStop = 9
            '    Return JudgeLotStop
            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_PLATE, UNIT_HI_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTHI Then
                    JudgeLotStop = 10
                    Return JudgeLotStop
                End If
            Else
                ' LOT�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_LOT, UNIT_HI_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTHI Then
                    JudgeLotStop = 11
                    Return JudgeLotStop
                End If

            End If
        End If

        ' FT-LO����
        If JudgeNgRate.CheckFTLO = True Then
            'If JudgeNgRate.SelectUnit = UNIT_BLOCK Then
            '    ' Block�P�ʂ̏W�v�Ŕ���
            '    JudgeLotStop = 12
            '    Return JudgeLotStop
            'Else
            If JudgeNgRate.SelectUnit = UNIT_PLATE Then
                ' Plate�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_PLATE, UNIT_LO_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTLO Then
                    JudgeLotStop = 13
                    Return JudgeLotStop
                End If
            Else
                ' LOT�P�ʂ̏W�v�Ŕ���
                GetHiLoOpenCount(UNIT_LOT, UNIT_LO_NG, IT, FT)
                If FT >= JudgeNgRate.ValFTLO Then
                    JudgeLotStop = 14
                    Return JudgeLotStop
                End If

            End If
        End If

    End Function


    ''' <summary>
    ''' �W�v���N���A����֐�  'V4.9.0.0�@
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ClearTotalCount() As Integer

        Debug.WriteLine("�f�o�b�O�E���b�Z�[�W���o��DataClrButton_Click")
        TrimData.ResetLotData()
        'V4.9.0.0�C
        TrimData.SetLaserPower(0.0)
        TrimData.GetProductData(StatData(2).Label.Text, StatData(4).Label.Text, StatData(6).Label.Text, StatData(8).Label.Text, StatData(9).Label.Text, StatData(11).Label.Text, StatData(12).Label.Text, StatData(14).Label.Text)
        SimpleTrim_LoggingStart()
        CmdBlockData.Visible = False         ' �mBLOCK DATA�n�{�^�����펞�ɂ���B
        Call Form1.System1.OperationLogging(gSysPrm, MSG_OPLOG_CLRTOTAL, "MANUAL")
        Call Form1.ClearCounter(1)                              ' ���Y�Ǘ��f�[�^�̃N���A
        Call ClrTrimPrnData()                                   ' ���ݸތ��ʈ�����ڂ��ް���ر����(���[���a����) V1.18.0.0�B

        ' ���v�\����ON�̏ꍇ�A�\�����X�V����
        'If Form1.chkDistributeOnOff.Checked = True Then
        gObjFrmDistribute.RedrawGraph() 'V4.0.0.0-81

    End Function

    ''V5.0.0.4�D��
    ''' <summary>
    ''' ��R���Ƃ̔���F�\���p
    ''' </summary>
    ''' <returns>True:����AFalse�ُ�</returns>
    ''' <remarks></remarks>
    Public Function IsTrimJudge(ByVal mode As Integer, ByVal ResNo As Integer) As Boolean

        IsTrimJudge = False
        If (mode = 0) Then      ' Initial 
            If ((gwTrimResult(ResNo - 1) = TRIM_RESULT_OK) Or (gwTrimResult(ResNo - 1) = TRIM_RESULT_FT_HING) Or (gwTrimResult(ResNo - 1) = TRIM_RESULT_FT_LONG)) Then
                IsTrimJudge = True
            End If
        Else        ' Final 
            If (gwTrimResult(ResNo - 1) = TRIM_RESULT_OK) Then
                IsTrimJudge = True
            End If
        End If

    End Function
    ''V5.0.0.4�D��

#If START_KEY_SOFT Then
#Region "�\�t�gSTART�ARESET�{�^������"


    Private Sub SoftStartButton_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles SoftStartButton.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            e.IsInputKey = True
            Form1.CbDigSwH.Focus()
        End If
    End Sub

    Private Sub SoftStartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoftStartButton.Click
        Try
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call START_SWITCH_ON()
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub

    Private Sub SoftResetButton_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles SoftResetButton.PreviewKeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
            e.IsInputKey = True
            Form1.CbDigSwH.Focus()
        End If
    End Sub

    Private Sub SoftResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoftResetButton.Click
        Try
#If START_KEY_SOFT Then
            If gbStartKeySoft Then
                Call RESET_SWITCH_ON()
            End If
#End If
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftResetButton_Click() TRAP ERROR = " + ex.Message)
        End Try

    End Sub
#End Region

#Region "�\�t�g�{�^���\����\��"
    Public Sub Sub_StartResetButtonDispOn()
        Try
            SoftStartButton.Enabled = True
            SoftStartButton.Visible = True
            SoftResetButton.Enabled = True
            SoftResetButton.Visible = True
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButtonDispON() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
    Public Sub Sub_StartResetButtonDispOff()
        Try
            SoftStartButton.Enabled = False
            SoftStartButton.Visible = False
            SoftResetButton.Enabled = False
            SoftResetButton.Visible = False
        Catch ex As Exception
            MsgBox("SimpleTrimmer.SoftStartButtonDispOFF() TRAP ERROR = " + ex.Message)
        End Try
    End Sub
#End Region

#End If

End Module


'=============================== END OF FILE ===============================