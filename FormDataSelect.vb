'===============================================================================
'   Description  : データ選択画面処理(自動運転用)
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Imports TKY_ALL_SL432HW.My.Resources    'V4.4.0.0-0

Public Class FormDataSelect
#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    Private Const DATA_DIR_PATH As String = "C:\TRIMDATA\DATA"          ' データファイルフォルダ(既定値)

    '----- 動作モードの最大ファイル数 -----
    Private Const COUNT_MAGAZINE As Integer = 3                         ' マガジンモード(最大3つの供給マガジン)
    Private Const COUNT_LOT As Integer = 30                             ' ロットモード(最大3つの供給マガジン×10ロット)
    Private Const COUNT_ENDLESS As Integer = 1                          ' エンドレスモード
    Private Const COUNT_MAX As Integer = 30                             ' 最大ファイル数

    '----- 変数定義 -----
    Private mExitFlag As Integer                                        ' 終了フラグ

#End Region

#Region "【メソッド定義】"
#Region "終了結果を返す"
    '''=========================================================================
    ''' <summary>終了結果を返す</summary>
    ''' <returns>cFRS_ERR_START = OKボタン押下
    '''          cFRS_ERR_RST   = Cancelボタン押下</returns>
    '''=========================================================================
    Public Function sGetReturn() As Integer

        Dim strMSG As String

        Try
            Return (mExitFlag)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.sGetReturn() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Function
#End Region

#Region "ShowDialogメソッドに独自の引数を追加する"
    '''=========================================================================
    ''' <summary>ShowDialogメソッドに独自の引数を追加する</summary>
    ''' <param name="Owner">(INP)未使用</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Public Overloads Sub ShowDialog(ByVal Owner As IWin32Window)

        Dim strMSG As String

        Try
            ' 初期処理
            mExitFlag = -1                                              ' 終了フラグ = 初期化

            ' 画面表示
            Me.ShowDialog()                                             ' 画面表示
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.ShowDialog() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Form_Load時処理"
    '''=========================================================================
    ''' <summary>Form_Load時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FormDataSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strMSG As String
        Dim lSts As Long = 0                                            ' ###206
        Dim r As Integer = 0                                            ' ###206

        Try
            ' フォーム名 
            'Me.Text = MSG_AUTO_14                                       ' "データ選択"

            ' 動作モード選択ラジオボタン
            'GrpMode.Text = MSG_AUTO_01                                  ' "動作モード"
            'BtnMdMagazine.Text = MSG_AUTO_02                            ' "マガジンモード"
            'BtnMdLot.Text = MSG_AUTO_03                                 ' "ロットモード"
            'BtnMdEndless.Text = MSG_AUTO_04                             ' "エンドレスモード"
            ' ラベル名・ボタン名 
            'LblDataFile.Text = MSG_AUTO_05                              ' "データファイル"
            'LblListList.Text = MSG_AUTO_06                              ' "登録済みデータファイル"
            'BtnUp.Text = MSG_AUTO_07                                    ' "リストの1つ上へ"
            'BtnDown.Text = MSG_AUTO_08                                  ' "リストの1つ下へ"
            'BtnDelete.Text = MSG_AUTO_09                                ' "リストから削除"
            'BtnClear.Text = MSG_AUTO_10                                 ' "リストをクリア"
            'BtnSelect.Text = MSG_AUTO_11                                ' "↓登録↓"
            'BtnOK.Text = MSG_AUTO_12                                    ' "OK"
            'BtnCancel.Text = MSG_AUTO_13                                ' "キャンセル"

            ' 動作モード選択ラジオボタン初期化
            '----- ###206↓ -----
            r = H_Read(LOFS_H00, lSts)                                  ' 初期設定状態取得(H0.00-H0.15)
            If (lSts And LHST_MAGAZINE) Then                            ' 4マガジン ?
                BtnMdEndless.Visible = True                             ' エンドレスモード表示 
                BtnMdMagazine.Visible = True                            ' マガジンモード表示 
                BtnMdLot.Visible = False                                ' ロットモード非表示 
                BtnMdEndless.Checked = True                             ' エンドレスモードにチェックマークを設定する
            Else
                BtnMdEndless.Visible = True                             ' エンドレスモード表示 
                BtnMdMagazine.Visible = False                           ' マガジンモード非表示 
                BtnMdLot.Visible = False                                ' ロットモード非表示 
                BtnMdEndless.Checked = True                             ' エンドレスモードにチェックマークを設定する
            End If
            'If (giActMode = MODE_MAGAZINE) Then
            '    BtnMdMagazine.Checked = True                            ' マガジンモード
            'ElseIf (giActMode = MODE_LOT) Then
            '    BtnMdLot.Checked = True                                 ' ロットモード
            'Else
            '    BtnMdEndless.Checked = True                             ' エンドレスモード
            'End If
            '----- ###206↑ -----

            ' リストボックスクリア
            Call ListList.Items.Clear()                                 ' 「登録済みデータファイル」リストボックスクリア

            '----- V1.18.0.0⑧↓ -----
            ' 登録済みデータファイルリストボックスにロード済みデータ名を表示する(オプション)
            If (giAutoModeDataSelect = 1) And (gStrTrimFileName <> "") Then ' ロード済みデータ名を表示する ?
                Call Sub_GetFileFolder(gStrTrimFileName, DrvListBox.Drive, DirListBox.Path)
                MakeFileList()                                              ' ←通常はDirListBox_Change()イベントが発生するので不要だが
                Call AddFileNameToListList(gStrTrimFileName)
            Else
                ' 「データファイル」リストボックスに日付付きファイル名を表示する
                DrvListBox.Drive = "C:"                                     ' ドライブ 
                DirListBox.Path = DATA_DIR_PATH                             ' ディレクトリリストボックス既定値
                MakeFileList()                                              ' ←通常はDirListBox_Change()イベントが発生するので不要だが
                '                                                           ' カレントが"C:\TRIMDATA\DATA"だと発生しないので必要
            End If
            '----- V1.18.0.0⑧↑ -----

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.FormDataSelect_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   ボタン押下時の処理
    '========================================================================================
#Region "OKボタン押下時処理"
    '''=========================================================================
    ''' <summary>OKボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click

        Dim r As Integer
        Dim Count As Integer
        Dim Idx As Integer
        Dim strDAT As String
        Dim strMSG As String = ""

        Try
            ' 選択リスト1以上有りかチェックする ?
            If (ListList.Items.Count < 1) Then
                '"データファイルを選択してください。"
                Call MsgBox(MSG_AUTO_18, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            ' エンドレスモードで選択リスト2以上有りならエラーとする
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count > 1) Then
                '"エンドレスモード時は複数のデータファイルは選択できません。"
                Call MsgBox(MSG_AUTO_17, MsgBoxStyle.OkOnly)
                Exit Sub
            End If

            ' 選択データに対応する加工条件ファイルが存在するかチェックする(FL時)
            For Idx = 0 To ListList.Items.Count - 1
                strDAT = FileLstBox.Path + "\" + ListList.Items(Idx)
                r = GetFLCndFileName(strDAT, strMSG, True)              ' 存在チェック 
                If (r <> SerialErrorCode.rRS_OK) Then                   ' 加工条件ファイルが存在しない ?
                    ' "加工条件ファイルが存在しません。(加工条件ファイル名)"
                    strMSG = MSG_AUTO_20 + "(" + strMSG + ")"
                    Call MsgBox(strMSG, MsgBoxStyle.OkOnly, "")
                    ListList.SelectedIndex = Idx
                    Call ListList_Click(sender, e)                      ' データファイル名をフルパスでラベルテキストボックスに設定する
                    Exit Sub
                End If
            Next Idx

            ' 連続運転用のデータファイル数とデータファイル名配列をグローバル領域に設定する
            giAutoDataFileNum = ListList.Items.Count                    ' データファイル数
            Count = giAutoDataFileNum
            If (Count < COUNT_MAX) Then
                Count = COUNT_MAX
            End If
            ReDim gsAutoDataFileFullPath(Count - 1)
            For Idx = 0 To giAutoDataFileNum - 1                        ' データファイル名
                gsAutoDataFileFullPath(Idx) = FileLstBox.Path + "\" + ListList.Items(Idx)
            Next

            ' 動作モード選択ラジオボタンより連続運転動作モードを設定する
            If (BtnMdMagazine.Checked = True) Then
                giActMode = MODE_MAGAZINE                               ' マガジンモード
            ElseIf (BtnMdLot.Checked = True) Then
                giActMode = MODE_LOT                                    ' ロットモード
            Else
                giActMode = MODE_ENDLESS                                ' エンドレスモード
            End If

            ' 動作モードにより自動運転するファイル数を再設定する
            Call Sub_AddFileName(giActMode)

            mExitFlag = cFRS_ERR_START                                  ' Return値 = OKボタン押下 

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnOK_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' フォームを閉じる
    End Sub
#End Region

#Region "動作モードにより自動運転するファイル数を再設定する"
    '''=========================================================================
    ''' <summary>動作モードにより自動運転するファイル数を再設定する</summary>
    ''' <param name="ActMode">(INP)動作モード</param>
    '''=========================================================================
    Private Sub Sub_AddFileName(ByVal ActMode As Integer)

        Dim Count As Integer
        Dim Idx As Integer
        Dim strFNAM As String
        Dim strMSG As String

        Try
            ' 動作モードにより自動運転するファイル数を再設定する
            If (ActMode = MODE_MAGAZINE) Then
                Count = COUNT_MAGAZINE                                  ' マガジンモード
            ElseIf (ActMode = MODE_LOT) Then
                Count = COUNT_LOT                                       ' ロットモード
            Else
                Count = COUNT_ENDLESS                                   ' エンドレスモード
            End If

            ' ファイル数が足りない分、最後のデータファイル名を設定する
            Idx = giAutoDataFileNum - 1                                 ' 画面指定のデータファイル数
            strFNAM = gsAutoDataFileFullPath(Idx)                       ' 画面指定の最後のデータファイル名 
            For Idx = giAutoDataFileNum To Count - 1
                gsAutoDataFileFullPath(Idx) = strFNAM
            Next Idx
            giAutoDataFileNum = Count

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.Sub_AddFileName() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "Cancelボタン押下時処理"
    '''=========================================================================
    ''' <summary>Cancelボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Dim strMSG As String

        Try
            mExitFlag = cFRS_ERR_RST                                    ' Return値 = Cancelボタン押下

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnCancel_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
        Me.Close()                                                      ' フォームを閉じる
    End Sub
#End Region

#Region "「リストの１つ上へ」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストの１つ上へ」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUp.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' １つ上のインデックスを指定する
            Idx = ListList.SelectedIndex
            If (Idx <= 0) Then Exit Sub
            ListList.SelectedIndex = Idx - 1

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            Call ListList_Click(sender, e)

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnUp_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「リストの１つ下へ」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストの１つ下へ」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDown.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' １つ下のインデックスを指定する
            Idx = ListList.SelectedIndex + 1
            If (Idx >= ListList.Items.Count) Then Exit Sub
            ListList.SelectedIndex = Idx

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            Call ListList_Click(sender, e)

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnDown_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「リストから削除」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストから削除」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' 「登録済みデータファイル」リストボックスから1つ削除する
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then Exit Sub
            ListList.Items.RemoveAt(Idx)                                  '「登録済みデータファイル」リストボックスから1項目削除(※Remove()は文字列指定)

            ' データファイル名をフルパスでラベルテキストボックスに設定する(削除の１つ前のデータを選択状態とする)
            If (Idx > 0) Then
                Idx = Idx - 1
                ListList.SelectedIndex = Idx
            End If
            Call ListList_Click(sender, e)

            ' エンドレスモード処理
            Call DspEndless()

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnDelete_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「リストをクリア」ボタン押下時処理"
    '''=========================================================================
    ''' <summary>「リストをクリア」ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>「登録済みデータファイル」リストボックスから全て削除</remarks>
    '''=========================================================================
    Private Sub BtnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClear.Click

        Dim r As Integer
        Dim strMSG As String

        Try
            ' 削除確認メッセージ表示
            If (ListList.Items.Count < 1) Then Exit Sub '               ' 登録リストなしならNOP 
            ' "登録リストを全て削除します。" & vbCrLf & "よろしいですか？"
            strMSG = MSG_AUTO_15 & vbCrLf & MSG_AUTO_16
            r = MsgBox(strMSG, MsgBoxStyle.OkCancel, "")
            If (r <> MsgBoxResult.Ok) Then Exit Sub

            ' 「登録済みデータファイル」リストボックスクリア
            Call ListList.Items.Clear()                                 '「登録済みデータファイル」リストボックスクリア

            ' データファイル名をフルパスでラベルテキストボックスに設定する(クリアする)
            Call ListList_Click(sender, e)

            ' エンドレスモード処理
            Call DspEndless()

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnClear_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "登録ボタン押下時処理"
    '''=========================================================================
    ''' <summary>登録ボタン押下時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelect.Click

        Dim Idx As Integer
        Dim Sz As Integer
        Dim Pos As Integer
        Dim strDAT As String
        Dim strMSG As String

        Try
            '「データファイル」リストボックスインデックス無効ならNOP
            Idx = ListFile.SelectedIndex
            If (Idx < 0) Then Exit Sub
            ' エンドレスモードで選択リスト1以上有りならNOP
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count >= 1) Then Exit Sub ' エンドレスモード ?

            ' 指定のデータファイル名を「登録済みデータファイル」リストボックスに追加する
            strDAT = ListFile.Items(Idx)                                ' 日付時刻付きファイル名なのでファイル名のみ取り出す 
            Sz = strDAT.Length
            Pos = strDAT.LastIndexOf(" ")
            If (Pos = -1) Then Exit Sub
            Pos = 19                                                    ' Pos = 19("YYYY/MM/DD HH:MM:ss ") ###225
            strDAT = strDAT.Substring(Pos + 1, Sz - Pos - 1)
            Idx = ListList.Items.Count
            Call ListList.Items.Add(strDAT)
            ListList.SelectedIndex = Idx

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            Call ListList_Click(sender, e)

            ' エンドレスモード処理
            Call DspEndless()

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.BtnSelect_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   リストボックスのクリックイベント処理
    '========================================================================================
#Region "「データファイル」リストボックスダブルクリックイベント処理"
    '''=========================================================================
    ''' <summary>「データファイル」リストボックスダブルクリックイベント処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListFile_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListFile.DoubleClick
        Dim strMSG As String

        Try
            ' 登録ボタン押下時処理へ
            Call BtnSelect_Click(sender, e)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.ListFile_DoubleClick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「登録済みデータファイル」リストボックスクリックイベント処理"
    '''=========================================================================
    ''' <summary>「登録済みデータファイル」リストボックスクリックイベント処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub ListList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListList.Click

        Dim Idx As Integer
        'Dim strDAT As String
        Dim strMSG As String

        Try
            ' 「登録済みデータファイル」リストボックスで選択されたデータファイル名をフルパスでラベルテキストボックスに設定する
            Idx = ListList.SelectedIndex
            If (Idx < 0) Then
                LblFullPath.Text = ""
            Else
                LblFullPath.Text = FileLstBox.Path + "\" + ListList.Items(Idx)
            End If
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.ListList_Click() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "「ドライブリストボックス」のクリックイベント処理"
    '''=========================================================================
    ''' <summary>「ドライブリストボックス」のクリックイベント処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DriveListBoxは非標準コントロールなのでツールボックスに追加する必要有り</remarks>
    '''=========================================================================
    Private Sub DrvListBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrvListBox.Click

        Dim strMSG As String

        Try
            ' ディレクトリリストボックスの選択ドライブを変更する
            DirListBox.Path = DrvListBox.Drive
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = ex.Message
            MsgBox(strMSG)
            DrvListBox.Drive = "C:"
        End Try
    End Sub
#End Region

#Region "「ディレクトリリストボックス」の変更時処理"
    '''=========================================================================
    ''' <summary>「ディレクトリリストボックス」の変更時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>DirListBoxは非標準コントロールなのでツールボックスに追加する必要有り</remarks>
    '''=========================================================================
    Private Sub DirListBox_Change(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DirListBox.Change

        Dim strMSG As String

        Try
            ' 選択ディレクトリを変更する(FileLstBoxは作業用のDummy)
            FileLstBox.Path = DirListBox.Path

            ' 「データファイル」リストボックスに日付時刻付きファイル名を表示する
            MakeFileList()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.DirListBox_Change() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '========================================================================================
    '   動作モードラジオボタン変更時の処理
    '========================================================================================
#Region "マガジンモード選択処理"
    '''=========================================================================
    ''' <summary>マガジンモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdMagazine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdMagazine.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

#Region "ロットモード選択処理"
    '''=========================================================================
    ''' <summary>ロットモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdLot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdLot.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

#Region "エンドレスモード選択処理"
    '''=========================================================================
    ''' <summary>エンドレスモード選択処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub BtnMdEndless_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMdEndless.CheckedChanged
        Call DspEndless()
    End Sub
#End Region

    '========================================================================================
    '   共通関数定義
    '========================================================================================
#Region "「データファイル」リストボックスに日付時刻付きファイル名を表示する"
    '''=========================================================================
    ''' <summary>「データファイル」リストボックスに日付時刻付きファイル名を表示する</summary>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub MakeFileList()

        Dim Count As Integer
        Dim i As Integer
        Dim Sz As Integer
        Dim strWK As String
        Dim strDAT As String
        Dim strMSG As String

        Try
            ' 「データファイル」リストボックスに日付時刻付きファイル名を表示する
            Call ListFile.Items.Clear()                                                 '「データファイル」リストボックスクリア
            Count = FileLstBox.Items.Count                                              ' ファイルの数 
            For i = 0 To (Count - 1)
                ' ファイル拡張子を設定
                If (gTkyKnd = KND_TKY) Then                                             ' TKYの場合(拡張性を考慮してTKYもサポートする)
                    strWK = ".tdt"
                ElseIf (gTkyKnd = KND_CHIP) Then                                        ' CHIPの場合
                    strWK = ".tdc"
                Else
                    strWK = ".tdn"                                                      ' NETの場合
                End If
                '----- V4.0.0.0⑨↓ -----
                If (giMachineKd = MACHINE_KD_RS) Then                                   ' SL436S ?
                    If (gTkyKnd = KND_TKY) Then
                        strWK = ".tdts"
                    ElseIf (gTkyKnd = KND_CHIP) Then
                        strWK = ".tdcs"
                    Else
                        strWK = ".tdns"
                    End If
                End If
                '----- V4.0.0.0⑨↑ -----

                ' 対象の拡張子でなければSKIP
                strDAT = FileLstBox.Items(i)
                Sz = strDAT.Length
                '-----V4.0.0.0⑨↓ -----
                If (giMachineKd = MACHINE_KD_RS) Then                                   ' SL436S ? 
                    If (Sz < 5) Then GoTo STP_NEXT
                    strDAT = strDAT.Substring(Sz - 5, 5)                                ' 拡張子を取り出す
                Else
                    If (Sz < 4) Then GoTo STP_NEXT
                    strDAT = strDAT.Substring(Sz - 4, 4)                                ' 拡張子を取り出す
                End If
                'If (Sz < 4) Then GoTo STP_NEXT
                'strDAT = strDAT.Substring(Sz - 4, 4)                                   ' 拡張子を取り出す
                '----- V4.0.0.0⑨↑ -----
                If (strDAT <> strWK) Then GoTo STP_NEXT '                               ' 対象の拡張子でなければSKIP

                ' 日付時刻付きファイルリスト作成
                strDAT = FileDateTime(FileLstBox.Path & "\" & FileLstBox.Items(i))
                Dim Dt As DateTime = DateTime.Parse(strDAT)
                strDAT = Dt.ToString("yyyy/MM/dd HH:mm:ss") + " " + FileLstBox.Items(i) ' 日付時刻の長さを合わせる 
                Call ListFile.Items.Add(strDAT)                                         ' 日付時刻付きファイル名を表示する
STP_NEXT:
            Next i
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.MakeFileList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "エンドレスモード処理"
    '''=========================================================================
    ''' <summary>エンドレスモード処理</summary>
    ''' <remarks>エンドレスモード時はデータファイルを１つしか選択できない</remarks>
    '''=========================================================================
    Private Sub DspEndless()

        Dim strMSG As String

        Try
            ' エンドレスモードで選択リスト1以上有りなら下記のボタン等を非活性化にする
            If (BtnMdEndless.Checked = True) And (ListList.Items.Count >= 1) Then
                ListFile.Enabled = False                                ' データファイルリストボックス非活性化
                BtnSelect.Enabled = False                               ' 登録ボタン非活性化 
                BtnUp.Enabled = False                                   '「リストの１つ上へ」ボタン非活性化
                BtnDown.Enabled = False                                 '「リストの１つ下へ」ボタン非活性化
            Else
                ListFile.Enabled = True                                 ' データファイルリストボックス活性化
                BtnSelect.Enabled = True                                ' 登録ボタン活性化 
                BtnUp.Enabled = True                                    '「リストの１つ上へ」ボタン活性化
                BtnDown.Enabled = True                                  '「リストの１つ下へ」ボタン活性化
            End If

            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.DspEndless() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0⑧↓ -----
#Region "登録済みデータファイルリストボックスにロード済みデータ名を表示する(オプション)"
    '''=========================================================================
    ''' <summary>登録済みデータファイルリストボックスにロード済みデータ名を表示する(オプション)</summary>
    ''' <param name="FileName">(INP)表示するファイル名</param>
    ''' <remarks></remarks>
    '''=========================================================================
    Private Sub AddFileNameToListList(ByRef FileName As String)

        Dim strDAT As String = ""
        Dim strMSG As String

        Try
            ' ファイル名なしならNOP
            If (FileName = "") Then Return

            ' 登録済みデータファイルリストボックスにロード済みデータ名を表示する
            Sub_GetFileName(FileName, strDAT)                           ' ファイル名のみを取り出す 
            Call ListList.Items.Add(strDAT)
            ListList.SelectedIndex = 0

            ' データファイル名をフルパスでラベルテキストボックスに設定する
            LblFullPath.Text = FileName

            ' エンドレスモード処理
            Call DspEndless()
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FormDataSelect.AddFileNameToListList() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region
    '----- V1.18.0.0⑧↑ -----
#End Region

End Class