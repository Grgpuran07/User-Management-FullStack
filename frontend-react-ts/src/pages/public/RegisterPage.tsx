import * as Yup from "yup";
import { useForm } from "react-hook-form";
import InputField from "../../components/general/InputField";
import { IRegisterDto } from "../../types/auth.types";
import { yupResolver } from "@hookform/resolvers/yup";
import useAuth from "../../hooks/useAuth.hooks";
import Button from "../../components/general/Button";
import { toast } from "react-hot-toast";
import { useState } from "react";
import { Link } from "react-router-dom";
import { PATH_PUBLIC } from "../../routes/paths";

const RegisterPage = () => {
  const [loading, setIsLoading] = useState(false);
  const { register } = useAuth();

  const registerSchema = Yup.object().shape({
    firstName: Yup.string().required("First Name is required."),
    lastName: Yup.string().required("Last Name is required."),
    userName: Yup.string().required("User Name is required."),
    email: Yup.string()
      .required("Email is required.")
      .email("Input text must be a valid mail."),
    password: Yup.string()
      .required("Password is required.")
      .min(8, "Password must be a valid mail."),
    address: Yup.string().required("Address is required."),
  });

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<IRegisterDto>({
    resolver: yupResolver(registerSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      userName: "",
      email: "",
      password: "",
      address: "",
    },
  });

  const onSubmitRegisterForm = async (data: IRegisterDto) => {
    try {
      setIsLoading(true);
      await register(
        data.firstName,
        data.lastName,
        data.userName,
        data.email,
        data.password,
        data.address
      );
      setIsLoading(false);
    } catch (error) {
      const err = error as { data: string; status: number };
      const { data, status } = err;
      if (status == 400 || status == 409) {
        toast.error(data);
      } else {
        toast.error("An error occured.Please contact admin.");
      }
    }
  };

  return (
    <div className="pageTemplate1">
      {/* left div */}
      <div className="max-sm:hidden flex-1 min-h-[600px] h-4/5 bg-gradient-to-tr from-[#DAC6FB] via-amber-400 to-[#AAC1F6] flex flex-col justify-center items-center rounded-l-2xl">
        <div className="h-3/5 p-6 rounded-2xl flex flex-col gap-8 justify-center items-start bg-white bg-opacity-20 border border-[#ffffff55] relative">
          <h1 className="text-6xl font-bold text-[#754eb4]">Dev Empower</h1>
          <h1 className="text-3xl font-bold text-[#754eb490]">
            A Home for developers
          </h1>
          <h4 className="text-3xl font-semibold text-white">
            Users Management
          </h4>
          <h4 className="text-2xl font-semibold text-white">V 1.0.0</h4>
          <div className="absolute -top-20 right-20 w-48 h-48 bg-gradient-to-br from-[#ef32d9]  to-[#89fffd] rounded-full blur-3xl"></div>
          <div className="absolute -bottom-20 right-20 w-32 h-32 bg-gradient-to-br from-[#cc2b5e] to-[#753a88] rounded-full blur-3xl"></div>
        </div>
      </div>
      {/* right div */}
    </div>
  );
};

export default RegisterPage;
