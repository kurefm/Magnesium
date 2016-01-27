# 接口设计描述

Magnesium作为最终的面向用户的控制类

## 使用方法
1. 初始化  
``` C#
var m = new Magnesium()     //初始化Magnesium
m.LoadConfig(path)          //加载配置文件
m.LoadDcp(dir/path)         //加载DCP
m.Run()                     //检查所有运行条件，确认无误后运行
```

2. 控制与使用  
在任何地方使用`Magnesium.Current`来访问Magnesium对象