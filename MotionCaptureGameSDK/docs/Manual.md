# 1 目录结构说明
![pic01](./pic/pic01.png)
- Plugins: 放dll的目录。Basic SDK引用放这里
- Prefab: sdk自带示例场景的prefab资源
- Resources: 其他资源
- Scenes: 场景
- Settings: 设置
- StandTravelModel: SDK程序集目录。包括Runtime、Editor、示例场景
- ThirdParty: 第三方引用package

# 2 SDK引入方法
(1) 右键选择整个Unity工程，Export Package导出

![pic02](./pic/pic02.png)

(2) 在想要引用SDK的Unity工程里面，右键选择Import Package → Custom Package

![pic03](./pic/pic03.png)

注: 如果引入工程前已经引入了RootMotion(FinalIK)工程，请先删除，否则引用SDK会出问题

(3) 导入后若要使用FinalIK作为动捕驱动角色运动的组件，需要在Edit → Project Setting中添加预定义字段“USE_FINAL_IK”

![pic04](./pic/pic04.png)

若不添加则默认使用Unity原生IK组件

# 3 SDK使用说明

StandTravelModel → Scene → TravelStandSDKTest为示例场景。
参考该场景下，TestMan的设置方式，挂载StandTravelModelManager脚本，进行相关配置后，可实现动捕控制角色
（需要本地已经部署、启动OS。OS部署启动方式请见OS相关文档）

![pic05](./pic/pic05.png)