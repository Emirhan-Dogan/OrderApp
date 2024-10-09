export interface IProductByCreateDTO{
    createdUserId:number,
    status:boolean,
    name:string,
    productCode:string,
    description:string,
    imagePath:string,
    price:number,
    sizeCode:string
    stockCount:number,
    colorId:number,
    categoryId:number,
    storageId:number
}