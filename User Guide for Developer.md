# 开发者说明书

## Magnesium构成
###概述
Magnesium大致上分为2个部分
1. 核心类库
核心类库位于Magnesium.Core项目下，其中包含Magnesium运行的全部基本组件，你可以抛弃GUI直接在命令行下运行核心库。
2. GUI
GUI位于Magnesium.Wpf中，提供了一些高级可视化组件，但这些组件与核心完全无关。抛弃这些，不影响核心的正常运行。

### 项目说明
1. **Sample**文件夹下提供了一个名为SaltTiger的数据采集插件示例。
2. **Solution Items**文件夹中存放了一些设计设计实现的简述。
3. **Magnesium.Core**项目为Magnesium的核心类库，是Magnesium的最核心部分，为其他部分的运行提供基本功能的支持。
4. **Magnesium.Core.Debug**项目为Magnesium.Core的手动测试内容。
5. **Magnesium.Crawler**项目已经废弃，为爬虫的早期实现。
6. **Magnesium.DataBase**项目已经废弃，为数据库早期的实现。
7. **Magnesium.Tests**项目为单元测试。
8. **Magnesium.Wpf**项目是Magnesium的GUI实现。

## 编写数据采集插件
### 概述
Magnesium支持数据采集器扩展插件，可以根据自己的需求来开发数据采集插件，然后加载到Magnesium中运行。

### 数据采集器构成
每个数据采集器都必须直接或间接实现**IDataCollectProvider**接口。一个数据采集器若要成功运行，至少需要实现发送请求(SendHandler)、解析数据(ParseHandler)和存储数据(StoreHandler)3个处理程序。为了确保正常运行，你还需要添加一个GUID来唯一标识你的数据采集插件。  
如果你希望你的数据采集器能够支持自动任务，你还需要实现一个RequestGenerator。

### Magnesium提供的基础类
Magnesium提供了如下基础类的支持。
1. HTTP请求，位于Magnesium.Core.Web命名空间中，提供请求HTTP数据的一般方法。
2. Sqlite3数据库，位于Magnesium.Core.DataBase.Sqlite命名空间中，提供了对Sqlite3操作的方法。
3. 日志记录器，位于Magnesium.Core.Log命名空间中，提供了日志记录的方法。日志可以在任意地方任意线程中使用，无需担心同步的问题。
4. 布隆过滤器，位于Magnesium.Core.Base命名空间中，Magnesium只提供了一个共用的布隆过滤器，你需要通过Magnesium.Current.BloomFilter来访问。但RequestGenerator中的虚方法直接默认检查了布隆过滤器，你无需自行再次访问。

### Tips
1. Magnesium已经对并发进行了处理，你无需担心并发可能会引发的异常。在Magnesium中，默认发送请求(SendHandler)会以多线程运行，而解析数据(ParseHandler)和存储数据(StoreHandler)会以单线程运行。
2. 你可以实现生成器界面(GeneratorUI)、设置界面(SettingUI)和数据查看界面(DataViewUI)来提高更好的用户体验。
3. Magnesium尚处于试验阶段，还有待完善，如发现BUG，欢迎反馈。