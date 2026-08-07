import type { ButtonHTMLAttributes, ReactNode } from "react";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  children: ReactNode;
}

export default function Button({
  children,
  className = "",
  ...props
}: ButtonProps) {
  return (
    <button
      className={`
        px-6
        py-3
        rounded-xl
        bg-purple-600
        text-white
        font-semibold
        transition
        hover:bg-purple-700
        ${className}
      `}
      {...props}
    >
      {children}
    </button>
  );
}
