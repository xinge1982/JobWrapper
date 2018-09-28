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