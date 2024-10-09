import { ICategory } from "./ICategory"
import { IColor } from "./IColor"
import { IStorage } from "./IStorage"

export interface IProduct{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
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
    color:IColor,
    colorId:number,
    category:ICategory,
    categoryId:number,
    storage:IStorage,
    storageId:number
}