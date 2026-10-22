FROM node:24-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN npx nuxt prepare

RUN npm run build

FROM node:24-alpine

WORKDIR /app

COPY --from=build /app/.output ./.output

USER node

EXPOSE 3000

CMD ["node", ".output/server/index.mjs"]
