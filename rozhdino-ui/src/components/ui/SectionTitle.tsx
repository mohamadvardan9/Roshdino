import type { ReactNode } from "react";

interface SectionTitleProps {
  title: string;
  description?: ReactNode;
}

export default function SectionTitle({
  title,
  description,
}: SectionTitleProps) {
  return (
    <div className="text-center mb-16">
      <h2 className="text-4xl font-black">{title}</h2>

      {description && <p className="mt-5 text-gray-600">{description}</p>}
    </div>
  );
}
