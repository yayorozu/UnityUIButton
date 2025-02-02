# UnityUIButton

UnityEngine.UI.Button にリアクションを付与したり、複数の色を同時に変更したい場合、
従来は Button コンポーネントに加え、各種補助コンポーネントを個別に追加する必要がありました。
しかし、プロジェクトの規模が大きくなったり、開発メンバーが増えると、これらの追加が抜けてしまうことが発生します。

そこで、ボタンの機能をモジュール化し、分かりやすく管理できるようにしました。

# 機能モジュールの一覧
以下の画像は、現在追加されているボタン機能モジュールの一覧例です。

<img src="https://cdn-ak.f.st-hatena.com/images/fotolife/h/hacchi_man/20200908/20200908032021.png" width="300" alt="ボタン機能モジュール一覧">

# モジュールの追加方法
機能を追加したい場合は、UniButtonModuleAbstract を継承したクラスを作成するだけで、
自動的にモジュール一覧に追加されます。

<img src="https://cdn-ak.f.st-hatena.com/images/fotolife/h/hacchi_man/20200908/20200908032617.png" width="300" alt="モジュール追加例">
以下は、音声再生機能を追加する例です。

```csharp
[Serializable]
public class ButtonSound : UniButtonModuleAbstract
{
    public override void Press()
    {
        Debug.Log("PlaySound");
    }
}
```

# ライセンス
本プロジェクトは MIT License の下でライセンスされています。<br>
詳細については、LICENSE ファイルをご覧ください。
