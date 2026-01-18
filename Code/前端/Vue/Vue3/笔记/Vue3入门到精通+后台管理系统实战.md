>Vue3 + Pinia +Vite +element-Plus +TS
> https://b23.tv/4biySW2

# 创建项目并设置启动自动打开浏览器、
> pnpm create vue@latest
> ![[Pasted image 20260118222105.png]]

# 引入ElementPlus并开启暗黑模式
> pnpm install element-plus

## 使用
```
import './assets/main.css'

  

import { createApp } from 'vue'

import { createPinia } from 'pinia'

import ElementPlus from 'element-plus'

import 'element-plus/dist/index.css'

import App from './App.vue'

import router from './router'

  

const app = createApp(App)

  

app.use(createPinia())

app.use(router)

app.use(ElementPlus)

  

app.mount('#app')
```