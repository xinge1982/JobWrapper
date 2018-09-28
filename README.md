# JobWrapper
为执行DLL中提供的函数而封装的命令行程序

## 使用方法
### 安装 .Net Framework 4.6

### 使用说明
可使用--help打印参数说明

   --dll=VALUE               DLL file path                        
   --class=VALUE             DLL class name                       
   --method=VALUE            DLL method name                      
   --json=VALUE              DLL parameter json                    
   --print                   Show DLL information                 
   --help                    Help about the command line interface


打印DLL文件类型及函数
```bash
JobWrapper.exe --file=TestLibrary.dll --print
```

调用TestLibrary.dll内的类TestLibrary.TestClass中的MultiHello函数，参数使用JSON格式数组表示
```bash
JobWrapper.exe --file=TestLibrary.dll --class=TestLibrary.TestClass --method=MultiHello --json=['deng',30,'tianjin']
```

## 程序说明
### JobWrapper.cs
1.程序启动，进入InteractiveRun函数，首先设置运行参数，使用OptionSet来处理输入参数。
2.通过optionSet.Parse(args)调用得到输入参数，确定需要执行的函数，并保存到actionToTake，最后执行actionToTake指向的函数。
3.ShowDynamic函数帮助打印DLL文件中的函数定义
4.LoadDynamic函数加载对应的DLL文件，按照输入的类名称实例化类
5.在类实例中找到对应的函数名称，利用JavaScriptSerializer将JSON格式参数解析为Object类型对象的数组
6.调用类实例中对应的函数，将Object类型对象的数组传入当作执行参数。

传入的参数使用JSON格式，测试时直接将实际参数组成的数组，没有按照JSON对象数组。如果需要按照对象形式，可参照JobParameter中定义的字段名称，使用JavaScriptSerializer解析JobParameter[]格式的JSON输入：
[{'value':'deng'},{'value':23},{'value':'tianjin'}]