import { IProductOrderByUpdateDTO } from "./IProductOrderByUpdateDTO";

export interface IOrderByUpdateDTO{
    lastUpdatedUserId:number,
    status:boolean,
    isDeleted:boolean,
    id:number,
    customerId:number,
    totalPrice:number,
    dateOfReceipt:Date,
    deliveryStatus:string,
    deliveryNotes:string,
    productOrders:IProductOrderByUpdateDTO[]
}