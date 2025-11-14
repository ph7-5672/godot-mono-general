# godot-mono-general
godot_mono版开发泛用模板。也可以看作一个简易框架，提供了一些简单工具和开发规范。
目前适配的godot版本：4.5.1。

## 架构分层

### 视图层（View Layer）
视图层的结构尽量细化，以最小单元访问逻辑层获取数据。
例如一个背包界面，每个背包格子都会调用`GetSlotData(index)`函数来获取数据。

### 逻辑层（Logic Layer）
逻辑层分为一个门面类（LogicFacade）和ECS（Entity-Component-System）。
门面类提供访问函数，只注重输入和返回数据，具体的逻辑在ECS中。
主要功能模块：
- 仓库/背包（Inventory）
- 存读档（SaveLoad）
- 成就（Achievement）

### 其他工具类：
- 单例工厂（SingletonFactory）
- 文件流读写（IOHelper）



## 代码规范
### 常量和枚举
全局常量和枚举写在逻辑层中的Constants类中。
局部常量和枚举写在使用它的类中。


