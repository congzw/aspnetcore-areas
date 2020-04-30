# how aspnetcore moudle areas works

- 1 hide areas files for .gitignore
- 2 exclude areas files for vs
- 3 set main site PostBuildEvent
- 4 add module startup code support in Common
- 5 create area project "Demo" in folder "src\MyApp.Web.Areas\"
- ref project, setup services, enjoy it!

## hide areas files for .gitignore

``` txt

  **/MyApp.Web/Areas

```

## exclude areas files for vs

``` xml
 
  <ItemGroup>
    <Compile Remove="Areas\**" />
    <Content Remove="Areas\**" />
    <EmbeddedResource Remove="Areas\**" />
    <None Remove="Areas\**" />
  </ItemGroup>

  ```

## set main site PostBuildEvent

``` xml

  <Target Name="CopyAreaFiles">
    <ItemGroup>
      <MyCopyAreaFiles Include="$(SolutionDir)\MyApp.Web.Areas\**\Content\**\*.*" />
      <MyCopyAreaFiles Include="$(SolutionDir)\MyApp.Web.Areas\**\Views\**\*.*" />
    </ItemGroup>
    <Copy SourceFiles="@(MyCopyAreaFiles)" DestinationFiles="@(MyCopyAreaFiles->'$(SolutionDir)\MyApp.Web\Areas\%(RecursiveDir)%(Filename)%(Extension)')" />
    <Message Text="----CopyAreaFiles完成----" Importance="high" />
  </Target>
  
  <!--fix vs call "CopyAreaFiles" failed because iis lock problems-->
  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <Touch Files="App_Offline.htm" AlwaysCreate="true" />
  </Target>

  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Delete Files="App_Offline.htm" />
    <CallTarget Targets="CopyAreaFiles" />
  </Target>

  ```

## api route conventions
  
- root_api: "Api/{Controller}/{Action}"
- area_api: "Api/{Area}/{Controller}/{Action}"

## view route conventions
 
- route_space: "Space/{user}/{controller}/{action}/{id?}"
- route_site_area: "{site}/{area:exists}/{controller}/{action}/{id?}"
- route_area:"{area:exists}/{controller}/{action}/{id?}"
- route_root:"{controller=Home}/{action=Index}/{id?}"