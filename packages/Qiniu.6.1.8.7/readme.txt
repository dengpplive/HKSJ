1.编译qiniu.dll，将它复制到lib/{framework}/下面({framework}可取Net20,Net40等)
2.修改qiniu.dll.nuspec中的相关参数
3.执行nuget pack Qiniu.dll.nuspec
4.执行nuget push Qiniu.dll.<x.x.x>.nupkg （<x.x.x>是本次发布的版本号，的qiniu.dll.nuspec中指定）

apikey:a9d1bc83-b285-4f6b-9bb1
此key失效。