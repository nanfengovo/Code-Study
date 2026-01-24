# 使用Canvas 画图的基本步骤
> 1、需要一个Canvas标签、
> 2、获取画笔对象
> 3、使用Canvas api

# 画一个方形
##  使用html标签
```HTML
<!DOCTYPE html>

<html lang="en">

<head>

    <meta charset="UTF-8">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>Canvas</title>

</head>

<body>

    <canvas id="canvas" width="600" height="400"></canvas>

    <script>

        //1、获取canvas 标签

        const canvas = document.getElementById('canvas');

        // 2、获取context （画笔） 对象

        const context = canvas.getContext('2d');

        // 3、 画出自己想要的图

        //画一个方形 有专门的api fillRect(x,y,width,height)

        context.fillRect(100,100,200,200)

    </script>

</body>

</html>
```
![[Pasted image 20260124221615.png]]

# 使用js
```html
<!DOCTYPE html>

<html lang="en">

<head>

    <meta charset="UTF-8">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>02 Canvas的基本使用2</title>

</head>

<body>

    <script>

        //1、 创建canvas 画布

        const canvas = document.createElement('canvas');

        //设置宽高

        canvas.width = 600;

        canvas.height = 400;

        document.body.append(canvas);

  

        //2、获取画笔

        const context = canvas.getContext('2d');

        //3、画出想要的图

        context.fillRect(100,100,150,100);

    </script>

</body>

</html>
```

** 注意** 
> canvas 元素为了正常显示里面的元素，不能用样式设置宽高要使用属性

# canvas api
## 画直线
```html
<!DOCTYPE html>

<html lang="en">

<head>

    <meta charset="UTF-8">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>使用Canvas画直线</title>

</head>

<body>

    <script>

        //创建canvas对象

        const canvas = document.createElement('canvas');

        //设置canvas的宽度和高度

        canvas.width = 600;

        canvas.height = 400;

        document.body.append(canvas);

        //获取context对象

        const context = canvas.getContext('2d');

        //画直线

        //线的起点

        context.moveTo(100,100);

        //线的终点

        context.lineTo(300,100);

        //调用画线的方法

        context.stroke();

    </script>

</body>

</html>
```

## 画折线
```html
<!DOCTYPE html>

<html lang="en">

<head>

    <meta charset="UTF-8">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>折线</title>

</head>

<body>

    <script>

        const canvas = document.createElement('canvas');

        canvas.width = 600;

        canvas.height = 400;

        document.body.append(canvas);

        const context = canvas.getContext('2d');

        context.moveTo(100,100);

        context.lineTo(300,200);

        context.lineTo(400,100);

        context.lineTo(500,300);

        context.lineTo(600,100);

        context.stroke();

    </script>

</body>

</html>
```
![[Pasted image 20260125004008.png]]