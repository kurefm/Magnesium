# 主要用到的正则表达式

## 去除htlm标签
`</?\s*?(?:[A-Za-z][^\s>/]*)(?:[^>""\']|""[^""]*""|\'[^\']*\')*>`

## 采集Album信息
暂时可以解析歌曲列表

`class="trackid">(\d{2})[\s\S]*?class="song_name"[\s\S]*?<a[^>]*?>(.*?)</a>\s*?([\s\S]*?)\s*?</td>[\s\S]*?class="song_hot">(\d*?)</td>`

新方案：按页面的内容顺序
1. 标题/子标题 
`id="title"[\s\S]*?h1[^>]*?>(?<Title>.*?)<(?:span>(?<Subtitle>.*?)<)?`

2. 评分
`v:rating[\s\S]*?<em[^>]*?>(?<Rating>[^<]*?)</em>`

3. 专辑基本信息
`艺人：[\s\S]*?<a[^>]*?[^>]*?/artist/(?<ArtistId>\d+)[^>]*?>[\s\S]*?语种：[\s\S]*?<td[^>]*?>(?<Language>[^<]*?)<[\s\S]*?唱片公司：[\s\S]*?<a[^>]*?>(?<Publisher>[^<]*?)<[\s\S]*?发行时间：[\s\S]*?<td[^>]*?>(?<PublishDate>[^>]*?)<[\s\S]*?(?:专辑类别：[\s\S]*?<td[^>]*?>(?<MediaType>[^<]*?)<[\s\S]*?)?(?:专辑风格：[\s\S]*?<a[^>]*?>(?<Genre>[^<]*?)<[\s\S]*?)?table`
4. 封面
`id="album_cover">[\s\S]*?href="(?<ImagePath>[^"]*?)"`

0. Id
`albumid[^']*?'(?<XId>\d+?)'`

5. 分享数量
`分享<em>\((?<ShareCount>\d+?)\)</em>`

6. 专辑介绍
`(?:v:summary[^>]*?>(?<Introduction>[\s\S]*?)<div[^>]*?album_intro_toggle[\s\S]*?)?`

7. 收藏数和评论数
`>(?<LikeCount>\d+?)[\D]*?<span>收藏</span>[\s\S]*?(?<CommentCount>\d+?)[\D]*?<span>评论</span>`



组合，组合后表达式特征明显，匹配效率更高

``` Regex
id="title"[\s\S]*?h1[^>]*?>(?<Title>.*?)<(?:span>(?<Subtitle>.*?)<)?[\s\S]*?v:rating[\s\S]*?<em[^>]*?>(?<Rating>[^<]*?)</em>[\s\S]*?艺人：[\s\S]*?<a[^>]*?[^>]*?/artist/(?<ArtistId>\d+)[^>]*?>[\s\S]*?语种：[\s\S]*?<td[^>]*?>(?<Language>[^<]*?)<[\s\S]*?唱片公司：[\s\S]*?<a[^>]*?>(?<Publisher>[^<]*?)<[\s\S]*?发行时间：[\s\S]*?<td[^>]*?>(?<PublishDate>[^>]*?)<[\s\S]*?(?:专辑类别：[\s\S]*?<td[^>]*?>(?<MediaType>[^<]*?)<[\s\S]*?)?(?:专辑风格：[\s\S]*?<a[^>]*?>(?<Genre>[^<]*?)<[\s\S]*?)?table[\s\S]*?id="album_cover">[\s\S]*?href="(?<ImagePath>[^"]*?)"[\s\S]*?albumid[^']*?'(?<XId>\d+?)'[\s\S]*?分享<em>\((?<ShareCount>\d+?)\)</em>[\s\S]*?(?:v:summary[^>]*?>(?<Introduction>[\s\S]*?)<div[^>]*?album_intro_toggle[\s\S]*?)?>(?<LikeCount>\d+?)[\D]*?<span>收藏</span>[\s\S]*?(?<CommentCount>\d+?)[\D]*?<span>评论</span>
```


## 解析salttiger书籍的正则表达式 

1.标题  
内容: `<h1 class="entry-title">Security for Web Developers</h1>`  
正则: `<h1[^>]*?entry-title[^>]*?>(?<Title>[\s\S]*?)</h1>` 

2.基本信息[待定]  
内容:  
``` HTML
<div class="entry-content">
    <p><img alt="" src="http://www.salttiger.com/wp-content/uploads/2015/11/16.jpg" />
        <br />
        <strong>出版时间：2015.11<br />
官网链接：<a href="http://shop.oreilly.com/product/0636920041429.do#" target="_blank">O’Reilly</a><br />
下载地址：<a href="http://pan.baidu.com/s/1hqwIDHM" target="_blank">百度网盘(PDF+EPUB+MOBI)</a></strong></p>
    <p><span id="more-6145"></span></p>
    <p>内容简介：</p>
    <p>As a web developer, you may not want to spend time making your web app secure, but it definitely comes with the territory. This practical guide provides you with the latest information on how to thwart security threats at several levels, including new areas such as microservices. You’ll learn how to help protect your app no matter where it runs, from the latest smartphone to an older desktop, and everything in between.</p>
    <p>Author John Paul Mueller delivers specific advice as well as several security programming examples for developers with a good knowledge of CSS3, HTML5, and JavaScript. In five separate sections, this book shows you how to protect against viruses, DDoS attacks, security breaches, and other nasty intrusions.</p>
    <ul>
        <li>Create a security plan for your organization that takes the latest devices and user needs into account</li>
        <li>Develop secure interfaces, and safely incorporate third-party code from libraries, APIs, and microservices</li>
        <li>Use sandboxing techniques, in-house and third-party testing techniques, and learn to think like a hacker</li>
        <li>Implement a maintenance cycle by determining when and how to update your application software</li>
        <li>Learn techniques for efficiently tracking security threats as well as training requirements that your organization can use</li>
    </ul>
</div>
```
正则: `<div[^>]*?entry-content[^>]*?>([\s\S]*?)</div>`  
出版时间: `(\d{4}\.\d{1,2})[\s]*?<br`  
官方地址: `<a[\s\S]*?href[\s\S]*?["'](?<Href>[^"^']*?)["'][\s\S]*?>(?<Name>[\s\S]*?)</a>`(第一条为官方地址，其余为下载链接)    
介绍: `/strong[\s\S]*?/p[^>]*?>([\s\S]+</p>)`  


3.分类及标签
内容: 
``` HTML
<footer class="entry-meta">
    本条目发布于
    <a href="http://www.salttiger.com/security-for-web-developers/" title="上午 8:59" rel="bookmark">
        <time class="entry-date" datetime="2015-11-19T08:59:55+00:00">2015 年 11 月 19 日</time>
    </a>。属于<a href="http://www.salttiger.com/category/ebooks/computer-science/oreilly/" title="查看O'Reilly中的全部文章" rel="category tag">O'Reilly</a>、<a href="http://www.salttiger.com/category/ebooks/" title="查看电子书中的全部文章" rel="category tag">电子书</a>、<a href="http://www.salttiger.com/category/ebooks/computer-science/" title="查看计算机中的全部文章" rel="category tag">计算机</a>分类，被贴了 <a href="http://www.salttiger.com/tag/security/" rel="tag">Security</a>、<a href="http://www.salttiger.com/tag/web-development/" rel="tag">Web Development</a> 标签。<span class="by-author">作者是<span class="author vcard"><a class="url fn n" href="http://www.salttiger.com/author/admin/" title="查看所有由SaltTiger发布的文章" rel="author">SaltTiger</a></span>。</span>
</footer>
```
正则: `<footer[^>]*?entry-meta[^>]*?>([\s\S]*?)</footer>`  
分类: `<a[^>]*?category[^>]*?>(?<Name>[\s\S]*?)</a>`  
标签: `<a[^>]*?"tag"[^>]*?>(?<Name>[\s\S]*?)</a>`  

4.解析所有链接
某个内容:
``` HTML
<li>01: <a href="http://www.salttiger.com/design-patterns-in-java-2nd-edition/">Design Patterns in Java, 2nd Edition</a> <span title="评论数量">(2)</span></li>
```
正则:  
``` Regex
<li>[\s\S]*?<a href="([^"]+?)">[\s\S]*?</a>[^>^<]*?<span title="评论数量">[\s\S]*?</span>[^>^<]*?</li>
```
