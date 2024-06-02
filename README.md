# Personalization Utils

<b>THIS PROJECT IS WORK IN PROGRESS !!!</b>

VRChat特定のユーザが入室したときにイベントを発火させたりすることができるパッケージ

対象プレイヤーをリストとして事前に登録しておき、条件に応じてEnable状態、Disable状態が切り替わります。
Enable状態のときに有効化されるGameObjectとDisable状態のときに有効化されるGameObjectをそれぞれ設定可能です。

## 導入方法

VCCよりパッケージのインポート

## 使用方法

1. Packages/com.mikinel.personalization-utils/Runtime/Prefabs/PersonalizationPlayerList.prefab をシーンに配置
2. Packages/com.mikinel.personalization-utils/Runtime/Prefabs/PersonalizationEvents.prefab をシーンに配置
3. PersonalizationEvents.prefab内のPersonalizationEventsコンポーネントにPersonalizationPlayerListを設定
4. PersonalizationEventsコンポーネントのModeを設定
    - OR : リストに含まれるプレイヤーの誰かが居るときにEnable状態になる
    - AND : リストに含まれるプレイヤーが全員いるときにEnable状態になる
    - Whitelist : リストに含まれるプレイヤー以外が居るときにDisable状態になる
5. PersonalizationEventsコンポーネントの対象オブジェクトを
    - EnableObjectArray : Enable状態のときに有効化されるGameObject
    - DisableObjectArray : Disable状態のときに有効化されるGameObject

## 更新履歴
[2024-06-02: v0.1.1] ベースの機能を作成、オブジェクトOnOffのみ動作

## ライセンス
MIT