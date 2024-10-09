import { IOrder } from "./IOrder";
import { IProduct } from "./IProduct";

export interface IProductOrder{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    price:number,
    unitInPrice:number,
    order:IOrder
    orderId:number,
    product:IProduct,
    productId:number
}