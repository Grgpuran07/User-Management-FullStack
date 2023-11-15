import * as Yup from "yup";
import { useForm } from "react-hook-form";
import InputField from "../../components/general/InputField";
import { ILoginDto } from "../../types/auth.types";
import { yupResolver } from "@hookform/resolvers/yup";
import useAuth from "../../hooks/useAuth.hooks";
import Button from "../../components/general/Button";
import { toast } from "react-hot-toast";
import { useState } from "react";
import { Link } from "react-router-dom";
import { PATH_PUBLIC } from "../../routes/paths";

const LoginPage = () => {
  const [loading, setIsLoading] = useState(false);
  const { login } = useAuth();

  const loginSchema = Yup.object().shape({
    userName: Yup.string().required("User Name is required."),
    password: Yup.string()
      .required("Password is required.")
      .min(8, "Password must be at least of 8 characters."),
  });

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<ILoginDto>({
    resolver: yupResolver(loginSchema),
    defaultValues: {
      userName: "",
      password: "",
    },
  });

  const onSubmitLoginForm = async (data: ILoginDto) => {
    try {
      setIsLoading(true);
      await login(data.userName, data.password);
      setIsLoading(false);
    } catch (error) {
      const err = error as { data: string; status: number };
      const { data, status } = err;

      if (status == 400 || status == 409) {
        toast.error(data);
      } else {
        toast.error("An error occured.Please contact admin.");
      }
      setIsLoading(false);
    }
  };

  return (
    <div className="flex h-[80vh] w-[100%] justify-center items-center">
      <form
        onSubmit={handleSubmit(onSubmitLoginForm)}
        className="min-h-[250px] w-[400px] p-[1rem] bg-[#f0ecf7] flex flex-col justify-center items-center rounded-r-2xl"
      >
        <h1 className="text-4xl font-bold mb-2 text-[#754eb4]">Login</h1>
        <InputField
          control={control}
          label="User Name"
          inputName="userName"
          error={errors.userName?.message}
        />
        <InputField
          control={control}
          label="Password"
          inputName="password"
          inputType="password"
          error={errors.password?.message}
        />
        <div className="px-4 mt-2 mb-6 w-9/12 flex gap-2">
          <h1>Don't have an account?</h1>
          <Link
            to={PATH_PUBLIC.register}
            className="text-[#754eb4] border-border-[#754eb4] hover:shadow-[0_0_5px_2px_#754eb44c] px-3 rounded-2xl duration-200 flex justify-center items-center"
          >
            Register
          </Link>
        </div>
        <div className="flex justify-center items-center gap-4 mt-6">
          <Button
            variant="secondary"
            type="button"
            label="Reset"
            onClick={() => {
              reset();
            }}
          />
          <Button
            variant="primary"
            type="submit"
            label="Login"
            onClick={() => {}}
            loading={loading}
          />
        </div>
      </form>
    </div>
  );
};

export default LoginPage;
