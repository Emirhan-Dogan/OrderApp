import { IAddressByCreateDTO } from "./IAddressByCreateDTO";

export interface ICustomerByCreateDTO{
    createdUserId:number,
    status:boolean,
    customerCode:string,
    citizenId:number,
    fullName:string,
    email:string,
    mobilePhones:string,
    birthDate:Date,
    gender:number,
    notes:string,
    imagePath:string,
    address:IAddressByCreateDTO
}