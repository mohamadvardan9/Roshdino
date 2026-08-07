import type { ReactNode } from "react";

interface BadgeProps {
  children: ReactNode;

  variant?: "primary" | "success" | "warning";
}

export default function Badge({
  children,

  variant = "primary",
}: BadgeProps) {
  const styles = {
    primary: "bg-purple-100 text-purple-600",

    success: "bg-green-100 text-green-600",

    warning: "bg-yellow-100 text-yellow-600",
  };

  return (
    <span
      className={`
    inline-flex
    items-center
    px-4
    py-2
    rounded-full
    text-sm
    font-medium
    ${styles[variant]}
`}
    >
      {children}
    </span>
  );
}
