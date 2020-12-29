# Awful.Web

## Because the world needed an Awful Proxy...

"Awful.Web" is an ASP.NET proxy-ish service for Something Awful, designed to let you access Something Awful on older browsers.

Using "Awful.NET" as its base, "Awful.Web" takes existing Something Awful pages and rewrites them as simple HTML tables, modeled after the [site's design from 2001](https://web.archive.org/web/20010802174429/http://forums.somethingawful.com/index.php). To get around SSL (which the vast majority of older browsers don't support), we attempt to proxy all file content through this service and serve it directly. Is it secure? Nope, not at all. But that's not the point of this project.

This project is in the vein of [The Old Net](https://theoldnet.com/), providing a hook into content older browsers can render. In this case, it's generating "new" content in an "old" way. 

![Screenshot from 2020-12-29 17-22-56](https://user-images.githubusercontent.com/898335/103319829-2a4c5380-4a01-11eb-8ac2-ebd664f229c7.png)
![Screenshot from 2020-12-29 17-23-06](https://user-images.githubusercontent.com/898335/103319830-2a4c5380-4a01-11eb-95d9-49db22386b93.png)
![Screenshot from 2020-12-29 17-36-58](https://user-images.githubusercontent.com/898335/103319831-2a4c5380-4a01-11eb-9b58-6629ce992409.png)
