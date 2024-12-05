# clklog-unity3d-demo

## 神策unity3d下载地址

<https://github.com/sensorsdata/sa-sdk-unity/releases>

## 神策unity3d集成文档

<https://manual.sensorsdata.cn/sa/3.0/zh_cn/tech_sdk_client_unity-17565840.html>

## ClkLog unity3d数据采集说明

- 在使用 ClkLog 作为 unity3d 的数据采集服务端时，需要注意以下内容

1. ClkLog的统计数据基于神策 sdk 的浏览页面事件和会话ID（$event_session_id）, 由于神策 unity3d sdk 的会话未实现，所以需要自己实现会话ID并配置为全局属性，同时关闭全埋点的浏览页面事件手动跟踪浏览页面事件。

2. 神策web、小程序、iOS、Android端的sdk浏览页面事件名称不同，请根据unity3d打包的版本去跟踪浏览页面事件，各端的浏览页面事件如下：
   - Web：$pageview
   - Android&iOS：$AppViewScreen
   - 微信小程序：$MPViewScreen
   - 其他端: ClkViewScreen (集成方法参考：<https://clklog.com/#/tutorials/ClkViewIntegrated>)

3. 在游戏中，可以将进入游戏场景时的动作作为页面浏览事件，即可对每次访问的场景次数作为浏览量进行统计。

## demo 包含的内容

1. unity3d的集成和初始化
2. 全埋点代码的接入
   - demo 中开启了开启 App 启动、App 退出全埋点事件。
3. 会话的实现与接入示例
   - 在每次demo游戏初始化时，都会生成和设置`$event_session_id`。
4. 简易用户的接入示例
   - 在demo中模拟了用户登录，在开始界面中的输入框进行用户名输入，点击Start按钮时视为登录。
5. 自定义用户属性的接入示例
   - 在模拟登录时，会同时模拟设置自定义用户属性。
6. 自定义事件的接入示例
   - 在demo中，在游戏加载场景时，触发并实现了`AppViewScreen`事件。
7. 自定义页面标题和路径的手动接入示例
   - 在点击Start按钮时，会加载到Main场景，触发 `AppViewScreen` 事件，并设置了`$title` 和 `$name` 事件属性。
   - 在点击Store按钮时，会加载到Shop场景，触发 `AppViewScreen` 事件，并设置了`$title` 和 `$name` 事件属性。
