'===============================================================================
'   Description  : ＩＯモニタ
'
'   Copyright(C) : OMRON LASERFRONT INC. 2011
'
'===============================================================================
Public Class FrmIOMon

#Region "【変数定義】"
    '===========================================================================
    '   変数定義
    '===========================================================================
    Private LblRBitAry(16) As Label                                     ' ラベル(入力ビット)
    Private LblWBitAry(16) As Label                                     ' ラベル(出力ビット)
    Private TimerRD As System.Threading.Timer = Nothing
    Private TimeVal As Integer
    Private mousePoint As Point                                         ' マウスのクリック位置

#End Region

#Region "【メソッド定義】"
#Region "Form_Load時処理"
    '''=========================================================================
    ''' <summary>Form_Load時処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ラベル配列設定(入力ビット)
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

            ' ラベル配列設定(出力ビット)
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

            ' ラベル配列初期化
            For Idx = 0 To 15
                LblRBitAry(Idx).BackColor = Color.White
                LblWBitAry(Idx).BackColor = Color.White
            Next Idx

            ' 初期表示位置
            Me.Location = New Point(Form1.Text4.Location.X, 0)

            ' タイマーオブジェクトの作成(TimerRD_Tickを100msec後にTimeVal msec間隔で実行する)
            TimeVal = 10                                                ' タイマー値(msec)
            TimerRD = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf TimerRD_Tick), Nothing, 100, TimeVal)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_Load() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "タイマーイベント(指定タイマ間隔が経過した時に発生)"
    '''=========================================================================
    ''' <summary>タイマーイベント(指定タイマ間隔が経過した時に発生)</summary>
    ''' <param name="Sts">(INP)</param>
    '''=========================================================================
    Private Sub TimerRD_Tick(ByVal Sts As Object)

        Dim strMSG As String

        Try
            ' コールバックメソッドの呼出しを停止する
            TimerRD.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)

            ' ＩＯモニタを表示する
            Call IOMonitor()

            ' タイマスタート 
            TimerRD.Change(TimeVal, TimeVal)
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmIOMon.TimerRD_Tick() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "ＩＯモニタを表示する"
    '''=========================================================================
    ''' <summary>ＩＯモニタを表示する</summary>
    '''=========================================================================
    Private Sub IOMonitor()

        Dim Idx As Integer
        Dim strMSG As String

        Try
            ' ローダ部受信データ
            ' 16BitデータのON/OFFをチェックする
            For Idx = 0 To 15
                If (gLdRDate And (2 ^ Idx)) Then
                    LblRBitAry(Idx).BackColor = Color.Lime
                Else
                    LblRBitAry(Idx).BackColor = Color.White
                End If
            Next Idx

            ' ローダ部送信データ
            ' 16BitデータのON/OFFをチェックする
            For Idx = 0 To 15
                If (gLdWDate And (2 ^ Idx)) Then
                    LblWBitAry(Idx).BackColor = Color.Red
                Else
                    LblWBitAry(Idx).BackColor = Color.White
                End If
            Next Idx
            Exit Sub

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmIOMon.IOMonitor() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

    '=========================================================================
    '   タイトルバーを持たないフォームはマウスで移動できないので
    '   下記を追加してマウスで移動できるようにする
    '=========================================================================
#Region "マウスのボタンが押されたとき"
    '''=========================================================================
    ''' <summary>マウスのボタンが押されたとき</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        Dim strMSG As String

        Try
            ' マウスの左ボタンが押されたときのマウスのクリック位置を記憶する
            If (e.Button And System.Windows.Forms.MouseButtons.Left) = System.Windows.Forms.MouseButtons.Left Then
                mousePoint = New Point(e.X, e.Y)                            ' 位置を記憶する
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_MouseDown() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#Region "マウスが動いたとき"
    '''=========================================================================
    ''' <summary>マウスが動いたとき</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    '''=========================================================================
    Private Sub FrmIOMon_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        Dim strMSG As String

        Try
            ' マウスの左ボタンを押下してドラッグした位置へフォームを移動する
            If (e.Button And System.Windows.Forms.MouseButtons.Left) = System.Windows.Forms.MouseButtons.Left Then
                Me.Left += e.X - mousePoint.X
                Me.Top += e.Y - mousePoint.Y
                'または、つぎのようにする
                'Me.Location = New Point(Me.Location.X + e.X - mousePoint.X, Me.Location.Y + e.Y - mousePoint.Y)
            End If

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "FrmIOMon.FrmIOMon_MouseMove() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
        End Try
    End Sub
#End Region

#End Region

End Class