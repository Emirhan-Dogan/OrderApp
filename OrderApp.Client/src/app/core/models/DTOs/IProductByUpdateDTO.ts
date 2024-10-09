export interface IProductByUpdateDTO{
    lastUpdatedUserId:number,
    status:boolean,
    isDeleted:boolean,
    id:number,
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