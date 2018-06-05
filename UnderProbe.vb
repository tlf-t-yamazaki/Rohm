'===============================================================================
'   Description  : 下方プローブ用にファイルを追加 V1.13.0.1①
'
'   Copyright(C) : OMRON LASERFRONT INC. 2013
'
'===============================================================================
Option Strict Off
Option Explicit On

Imports LaserFront.Trimmer.DefTrimFnc
Imports LaserFront.Trimmer.TrimData.DataManager     'V5.0.0.8①

Module UnderProbe

#Region "メソッドの定義"

#Region "下方プローブ有無チェック"
    ''=========================================================================
    '' <summary>下軸プローブの有無確認</summary>
    '' <returns>有り: True 無し: False</returns>
    '' <remarks>下軸プローブの有無確認</remarks>
    ''=========================================================================
    Public Function IsUnderProbe() As Boolean

        If (gSysPrm.stDEV.giPrbTyp And 2) = 0 Then                  ' Z2(下軸)なし？
            IsUnderProbe = False
        Else
            IsUnderProbe = True
        End If

    End Function
#End Region

#Region "下方Z2軸の動作"
    ''=========================================================================
    '' <summary>下軸Z2プローブのON/OFFを行う</summary>
    '' <param name="offon">(INP)ON/OFF指定</param>
    '' <returns>cFRS_NORMAL   = 正常
    ''          上記以外      = 非常停止等その他エラー
    '' </returns>
    ''=========================================================================
    Public Function Z2move(ByVal offon As Integer) As Integer

        Dim z As Double
        Dim r As Short
        Dim strMSG As String

        Try

            If offon = Z2ON Then
                z = typPlateInfo.dblLwPrbStpUpDist                          ' プローブＯＮ＝ＵＰ
            ElseIf offon = Z2STEP Then
                ' Z = 下方プローブステップ上昇距離－下方プローブステップ下降距離=ステップ上昇距離
                'z = typPlateInfo.dblLwPrbStpUpDist - typPlateInfo.dblLwPrbStpDwDist
                z = 0.0
                If z >= typPlateInfo.dblLwPrbStpUpDist Then                 ' 不定値なら下まで下げる。
                    z = 0
                End If
            Else
                z = 0                                                       ' プローブＯＦＦ＝ＤＯＷＮ
            End If

            ' Z2移動
            r = ZZMOVE2(z, 1)
            r = Form1.System1.EX_ZGETSRVSIGNAL(gSysPrm, r, 0)               ' エラーならメッセージ表示する   
            Return (r)

            ' トラップエラー発生時 
        Catch ex As Exception
            strMSG = "UnderProbe.Z2move() TRAP ERROR = " + ex.Message
            MsgBox(strMSG)
            Return (cERR_TRAP)                                              ' 戻値 = トラップエラー
        End Try
    End Function
#End Region

#End Region

End Module
